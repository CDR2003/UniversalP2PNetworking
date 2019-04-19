using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Upn
{
	internal class UpnSocket
	{
		private Socket _socket;

		internal UpnSocket()
		{
			_socket = new Socket( AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp );
			_socket.Blocking = false;
		}

		internal void Bind( IPAddress address, int port )
		{
			var endpoint = new IPEndPoint( address, port );
			_socket.Bind( endpoint );
		}

		internal int SendTo( byte[] data, int offset, int length, IPEndPoint remote )
		{
			return _socket.SendTo( data, offset, length, SocketFlags.None, remote );
		}
	}
}