using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundController : MonoBehaviour
{
	private SpriteRenderer sprite;
	private float widthWorld, heightWorld;
	private int widthPixel, heightPixel;
	private Color color; 

	void Start(){
		sprite = GetComponent<SpriteRenderer>(); 
		Texture2D texture = (Texture2D) Resources.Load("ground");
		Texture2D texture_clone = (Texture2D) Instantiate(texture);
		sprite.sprite = Sprite.Create(texture_clone, new Rect(0f, 0f, texture_clone.width, texture_clone.height), new Vector2(0.5f,0.5f), 10f);
		color = new Color(0f, 0f, 0f, 0f);
		InitSpriteDimensions();
		Bullet.groundController = this;

        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

	private void InitSpriteDimensions() {
		widthWorld = sprite.bounds.size.x;
		heightWorld = sprite.bounds.size.y;
		widthPixel = sprite.sprite.texture.width;
		heightPixel = sprite.sprite.texture.height;
	}

	public void DestroyGround( CircleCollider2D collider )
    {
		Debug.Log ("DestroyGround");
		V2int intPunkt = World2Pixel(collider.bounds.center.x, collider.bounds.center.y);
		int radius = Mathf.RoundToInt(collider.bounds.size.x*widthPixel/widthWorld);

		int x, y, px, nx, py, ny, d;
		
		for (x = 0; x <= radius; x++)
		{
			d = (int)Mathf.RoundToInt(Mathf.Sqrt(radius * radius - x * x));
			
			for (y = 0; y <= d; y++)
			{
				px = intPunkt.x + x;
				nx = intPunkt.x - x;
				py = intPunkt.y + y;
				ny = intPunkt.y - y;

				sprite.sprite.texture.SetPixel(px, py, color);
				sprite.sprite.texture.SetPixel(nx, py, color);
				sprite.sprite.texture.SetPixel(px, ny, color);
				sprite.sprite.texture.SetPixel(nx, ny, color);
			}
		}
		sprite.sprite.texture.Apply();
		Destroy(GetComponent<PolygonCollider2D>());
		gameObject.AddComponent<PolygonCollider2D>();
	}

	private V2int World2Pixel(float x, float y) {
		V2int punkt = new V2int();
		
		float dx = x-transform.position.x;
		punkt.x = Mathf.RoundToInt(0.5f*widthPixel+ dx*widthPixel/widthWorld);
		
		float dy = y - transform.position.y;
		punkt.y = Mathf.RoundToInt(0.5f*heightPixel + dy * heightPixel / heightWorld);
		
		return punkt;
	}
}
