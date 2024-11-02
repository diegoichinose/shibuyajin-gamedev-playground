using UnityEngine;

public class MakeParentTransparentOnCollisionWithPlayer : MonoBehaviour
{
	public float opacity = 0.2f;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			ChangeMyParentOpacity(opacity);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			ChangeMyParentOpacity(1.0f);
	}

	private void ChangeMyParentOpacity(float opacity)
	{
        SpriteRenderer thisObjectParentSprite = gameObject.GetComponentInParent<SpriteRenderer>();
		if (thisObjectParentSprite != null)
			thisObjectParentSprite.color = new Color(1.0f, 1.0f, 1.0f, opacity);
	}
}