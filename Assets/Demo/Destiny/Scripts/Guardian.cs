using UnityEngine;

namespace Demo.Destiny {
	public class Guardian : MonoBehaviour {
		public Rigidbody physicsObject;
		[Header("New params")]
		public Jump[] jumps;

		[System.Serializable]
		public class Jump {
			[Header("Impulse")]
			public bool cancelDownwardMomentum;
			public Vector3 momentumScale;
			public Vector3 impulseSpeed;
			[Header("Acceleration")]
			public float accelerationDuration;
			public Vector3 acceleration;
		}

		[Header("Params")]
		public float walkAcceleration = 10;
		public float idleDeceleration = 20;
		public float moveSpeedMax = 5;
		[Header("State")]
		public int jumpsDone;
		public bool isGrounded;
		public float moveSpeed;
		public bool wantToAccelerate;
		public float accelerationElapsed;

		private void OnCollisionEnter(Collision collision) {
			isGrounded = true;
			jumpsDone = 0;
			wantToAccelerate = false;
			accelerationElapsed = 0;
		}

		private void OnCollisionExit(Collision collision) {
			isGrounded = false;
		}

		private void Update () {
			int jumpIndex = Mathf.Min(jumpsDone, jumps.Length - 1);
			var settings = jumps[jumpIndex];

			bool jumpButtonDown = Input.GetKeyDown(KeyCode.Space);
			if (jumpButtonDown) {
				var velocity = physicsObject.velocity;

				if (!isGrounded) {
					wantToAccelerate = !wantToAccelerate;
				}

				if (wantToAccelerate) {
					if (settings.cancelDownwardMomentum && velocity.y < 0) {
						velocity.y = 0;
						physicsObject.velocity = velocity;
					}
				}

				bool canJump = jumpsDone < jumps.Length;
				if (canJump) {
					jumpsDone++;
					
					velocity = Vector3.Scale(velocity, settings.momentumScale);
					velocity = velocity + settings.impulseSpeed;
				}

				physicsObject.velocity = velocity;
			}

			bool canLift = accelerationElapsed < settings.accelerationDuration;
			if (canLift && wantToAccelerate) {
				accelerationElapsed += Time.deltaTime;
				physicsObject.AddForce(settings.acceleration, ForceMode.Acceleration);
			}

			bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
			bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
			
			if (isGrounded || moveLeft || moveRight) {
				float moveAcceleration = (moveLeft || moveRight) ? walkAcceleration : idleDeceleration;
				float moveDirection = (moveLeft ? -1 : 0) + (moveRight ? 1 : 0);
				float moveSpeedTarget = moveSpeedMax * moveDirection;
				moveSpeed = Mathf.MoveTowards(moveSpeed, moveSpeedTarget, moveAcceleration * Time.deltaTime);
			}

			var moveVector = new Vector3(moveSpeed, 0, 0);
			physicsObject.MovePosition(
				physicsObject.position + moveVector * Time.deltaTime
			);
		}
	}
}
