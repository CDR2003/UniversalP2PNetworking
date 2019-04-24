using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Upn
{
	internal class UpnSocket
	{
		internal static string LocalIP
		{
			get
			{
				var host = Dns.GetHostEntry( Dns.GetHostName() );
				foreach( var ip in host.AddressList )
				{
					if( ip.AddressFamily == AddressFamily.InterNetwork )
					{
						return ip.ToString();
					}
				}
				throw new System.Exception( "No network adapters with an IPv4 address in the system!" );
			}
		}

		internal bool IsStarted { get; private set; }

		internal int Available
		{
			get
			{
				return _socket.Available;
			}
		}

		private Socket _socket;

		internal UpnSocket()
		{
			this.IsStarted = false;
		}

		internal void Close()
		{
			if( this.IsStarted == false )
			{
				return;
			}

			_socket.Close();
			this.IsStarted = false;
		}

		internal void Start( int port )
		{
			_socket = new Socket( AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
			_socket.Blocking = false;

			var endpoint = new IPEndPoint( IPAddress.Any, port );
			_socket.Bind( endpoint );

			this.IsStarted = true;
		}

		internal int SendTo( byte[] data, int offset, int length, IPEndPoint remote )
		{
			return _socket.SendTo( data, offset, length, SocketFlags.None, remote );
		}

		internal int ReceiveFrom( byte[] data, int size, ref IPEndPoint remote )
		{
			EndPoint endPoint = new IPEndPoint( IPAddress.Any, 0 );
			var ret = _socket.ReceiveFrom( data, size, SocketFlags.None, ref endPoint );
			remote = endPoint as IPEndPoint;
			return ret;
		}
	}
}