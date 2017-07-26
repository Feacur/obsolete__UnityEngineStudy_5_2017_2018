using UnityEngine;

namespace Demo.Destiny {
	public class Guardian : MonoBehaviour {
		public Rigidbody physicsObject;
		[Header("Params")]
		public float jumpHeightMax = 1;
		public float walkAcceleration = 10;
		public float idleDeceleration = 20;
		public float moveSpeedMax = 5;
		public int jumpsLimit = 1;
		public bool canGlide;
		public Vector3 glideAcceleration = new Vector3(0, 7, 0);
		public float liftDurationMax;
		public Vector3 liftAcceleration = new Vector3(0, 13, 0);
		[Header("State")]
		public int jumpsDone;
		public bool isGrounded;
		public float moveSpeed;
		public bool isGliding;
		public bool isLifting;
		public float liftDuration;

		private void OnCollisionEnter(Collision collision) {
			isGrounded = true;
			jumpsDone = 0;
			isGliding = false;
			isLifting = false;
			liftDuration = 0;
		}

		private void OnCollisionExit(Collision collision) {
			isGrounded = false;
		}

		private void Update () {
			bool canJump = jumpsDone < jumpsLimit;
			bool jumpButtonDown = Input.GetKeyDown(KeyCode.Space);
			if (canJump && jumpButtonDown) {
				jumpsDone++;
				var jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * jumpHeightMax * 2);
				var jumpVector = new Vector3(0, jumpSpeed, 0);
				physicsObject.velocity = new Vector3(
					physicsObject.velocity.x,
					0, // (physicsObject.velocity.y < 0) ? physicsObject.velocity.y : 0,
					physicsObject.velocity.z);
				physicsObject.AddForce(jumpVector, ForceMode.VelocityChange);
			}

			if (!isGrounded && jumpButtonDown) {
				isGliding = !isGliding;
				isLifting = !isLifting;
			}

			if (canGlide && isGliding) {
				physicsObject.AddForce(glideAcceleration, ForceMode.Acceleration);
			}

			bool canLift = liftDuration < liftDurationMax;
			if (canLift && isLifting) {
				liftDuration += Time.deltaTime;
				physicsObject.AddForce(liftAcceleration, ForceMode.Acceleration);
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
