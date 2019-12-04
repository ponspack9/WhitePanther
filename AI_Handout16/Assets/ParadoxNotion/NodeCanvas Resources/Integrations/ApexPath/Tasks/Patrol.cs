using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Apex.Messages;
using Apex.Services;
using Apex.Steering;
using Apex.Steering.Behaviours;

namespace NodeCanvas.Tasks.ApexPath{

	public class Patrol : ApexBaseActionTask {

		public enum PatrolMode{
			Progressive,
			Random
		}

		[RequiredField]
		public BBParameter<PatrolPointsComponent> patrolRoute;
		public PatrolMode patrolMode;
		
		private int currentIndex = -1;

		protected override string info{
			get {return string.Format("{0} Patrol {1}", patrolMode, patrolRoute);}
		}

		protected override void OnExecute(){
			MoveNext();
		}

		protected override void OnUpdate(){
			var e = unit.lastNavigationEvent;
			if (e == UnitNavigationEventMessage.Event.None){
				return;
			}

			EndAction( e == UnitNavigationEventMessage.Event.DestinationReached );
		}

		void MoveNext(){
			var points = patrolRoute.value.worldPoints;

            if (patrolMode == PatrolMode.Random){
                var tmp = currentIndex;
                while (tmp == currentIndex){
                    currentIndex = Random.Range(0, points.Length - 1);
                }
            }

            if (patrolMode == PatrolMode.Progressive){
            	currentIndex = (int)Mathf.Repeat(currentIndex + 1, points.Length);
            }

            unit.MoveTo(points[ currentIndex ], false);
		}
	}
}