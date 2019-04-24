using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Upn.Examples.SocketUsage
{
	public class SocketUsageWindow : MonoBehaviour
	{
		public InputField txtYourIP;

		public InputField txtYourPort;

		public Button btnStart;

		public InputField txtTargetIP;

		public InputField txtTargetPort;

		public InputField txtMessage;

		public Button btnSend;

		private UpnSocket _socket;

		private void Awake()
		{
			btnStart.onClick.AddListener( this.OnBtnStartClicked );
			btnSend.onClick.AddListener( this.OnBtnSendClicked );

			_socket = new UpnSocket();
		}

		private void Start()
		{
			txtYourIP.text = UpnSocket.LocalIP;
			txtYourPort.text = UpnConfig.DefaultPort.ToString();
		}

		private void OnDestroy()
		{
			_socket.Close();

			this.btnStart.onClick.RemoveListener( this.OnBtnStartClicked );
			this.btnSend.onClick.RemoveListener( this.OnBtnSendClicked );
		}

		private void Update()
		{
			if( _socket.IsStarted == false )
			{
				return;
			}

			var buffer = new byte[UpnConfig.MaxPacketSize];
			IPEndPoint remote = null;
			while( _socket.Available > 0 )
			{
				var length = _socket.ReceiveFrom( buffer, _socket.Available, ref remote );
				if( length == 0 )
				{
					break;
				}

				using( var stream = new MemoryStream( buffer, 0, length, false ) )
				{
					var reader = new BinaryReader( stream );
					var message = reader.ReadString();
					Assert.AreEqual( stream.Position, stream.Length );

					Debug.LogFormat( "Message from '{0}': {1}", remote.ToString(), message );
				}
			}
		}

		private void OnBtnSendClicked()
		{
			var targetIP = IPAddress.Parse( txtTargetIP.text );
			var targetPort = int.Parse( txtTargetPort.text );
			var targetEndPoint = new IPEndPoint( targetIP, targetPort );

			using( var stream = new MemoryStream() )
			{
				var writer = new BinaryWriter( stream );
				writer.Write( txtMessage.text );
				_socket.SendTo( stream.GetBuffer(), 0, (int)stream.Position, targetEndPoint );
			}
		}

		private void OnBtnStartClicked()
		{
			var port = int.Parse( txtYourPort.text );
			_socket.Start( port );

			btnStart.interactable = false;
		}
	}
}