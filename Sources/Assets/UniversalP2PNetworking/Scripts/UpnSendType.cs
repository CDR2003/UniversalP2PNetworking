using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upn
{
	public enum UpnSendType
	{
		Unreliable,
		UnreliableNoDelay,
		Reliable,
		ReliableWithBuffering,
	}
}