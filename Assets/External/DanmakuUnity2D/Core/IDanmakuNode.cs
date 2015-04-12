using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D  {

	public interface IDanmakuNode {

		string NodeName {
			get;
		}

		Color NodeColor {
			get;
		}

		bool Connect(IDanmakuNode node);

	}
}