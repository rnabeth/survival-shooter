using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;
	public int currentHealth;
	public float sinkSpeed = 2.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;

	Animator anim;
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	CapsuleCollider capsuleCollider;
	bool isDead;
	bool isSinking;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		enemyAudio = GetComponent <AudioSource> ();
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();

		currentHealth = startingHealth;
	}

	void Update ()
	{
		if (isSinking) {
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}

	public void TakeDamage (int amount, Vector3 hitPoint)
	{
		if (isDead) {
			return;
		}

		enemyAudio.Play ();

		currentHealth -= amount;
            
		hitParticles.transform.position = hitPoint;
		hitParticles.Play ();

		if (currentHealth <= 0) {
			Death ();
		}
	}

	void Death ()
	{
		isDead = true;

		//You don't actually hit triggers, so the object is not an obstacle anymore
		capsuleCollider.isTrigger = true;

		anim.SetTrigger ("Dead");

		ScoreManager.score += scoreValue;
		
		enemyAudio.clip = deathClip;
		enemyAudio.Play ();
	}

	public void StartSinking ()
	{
		GetComponent <NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true; //When moving a collider in the scene Unity recalculates the static geometry if object is not kinematic
		isSinking = true;
		Destroy (gameObject, 2f); //Finished sinking after 2 seconds so we can destroy it
	}
}
