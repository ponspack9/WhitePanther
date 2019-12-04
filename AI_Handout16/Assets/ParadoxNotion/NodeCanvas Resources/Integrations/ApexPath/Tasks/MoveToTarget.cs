using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Apex.Messages;
using Apex.Services;
using Apex.Steering;

namespace NodeCanvas.Tasks.ApexPath{

	[Name("Seek (GameObject)")]
	public class MoveToTarget : ApexBaseActionTask {

		[RequiredField]
		public BBParameter<GameObject> target;
		
		private Vector3? lastRequest;
		
		protected override string info{
			get {return string.Format("Seek {0}", target);}
		}

		protected override void OnUpdate(){
			var targetPos = target.value.transform.position;
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