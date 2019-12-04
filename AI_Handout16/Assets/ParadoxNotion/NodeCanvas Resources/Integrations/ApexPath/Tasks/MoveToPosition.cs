using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Apex.Messages;
using Apex.Services;
using Apex.Steering;

namespace NodeCanvas.Tasks.ApexPath{

	[Name("Seek (Vector3)")]
	public class MoveToPosition : ApexBaseActionTask {

		public BBParameter<Vector3> targetPosition;
		
		private Vector3? lastRequest;
		
		protected override string info{
			get {return string.Format("Seek {0}", targetPosition);}
		}

		protected override void OnUpdate(){
			var targetPos = targetPosition.value;
			if (lastRequest != targetPos){
				lastRequest = targetPos;
				unit.MoveTo(targetPos, false);
			}

			var e = unit.lastNavigationEvent;
			if (e == UnitNavigationEventMessage.Event.None){
				return;
			}

			EndAction( e == UnitNavigationEventMessage.Event.DestinationReached );
		}

		protected override void OnStop(){
			base.OnStop();
			lastRequest = null;
		}
	}
}