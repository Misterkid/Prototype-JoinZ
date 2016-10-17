using UnityEngine;
using System.Collections;

public class GunBase : WeaponBase 
{
    public BulletBase gunBullet;

    public int maxMegazineSize;
    public int bullets;
    public float delay = 0.5f;
    public float reloadDelay = 2f;
    public float spread = 0.3f;
	public int bulletPerShot = 1;
    public int megazine;
    public int bulletsInMeg;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public Mover holder;
    public Texture scopeTexture;
    public Vector2 scopeTextureSize;
    public float fieldOfViewZoom = 45;
    private Timer shootTimer;
    private Timer reloadTimer;
    private bool canFire = true;
	// Use this for initialization
	void Start () 
    {
        shootTimer = this.gameObject.AddComponent<Timer>();
        reloadTimer = this.gameObject.AddComponent<Timer>();
        //ReloadIt();
        Reload();
	}
    public override void UseItem(Mover mover)
    {
        mover.holdingItem = this;
        this.holder = mover; 
       // mover.holdingGun.holder = mover;
        this.gameObject.transform.parent = mover.gunPosition.transform;
        this.gameObject.transform.position = mover.gunPosition.transform.position;
        this.gameObject.transform.rotation = mover.gunPosition.transform.rotation;

        base.UseItem(mover);
    }
    public void Shoot()//Douchebags.
    {
        if (canFire)
        {
            //ShootIt();
            if (bulletsInMeg > 0)
            {
                for (int i = 0; i < bulletPerShot; i++)
                {
                    if (bulletsInMeg > 0)
                    {
                        GameObject newBullet = GameObject.Instantiate(gunBullet.gameObject, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                        newBullet.GetComponent<BulletBase>().Move(spread, holder);
                        bulletsInMeg--;
                    }
                }
                AudioSource.PlayClipAtPoint(shootSound, this.gameObject.transform.position);
                //audioSource.Play();
            }
            else
            {
                Reload();
            }
            canFire = false;
        }
        if (!shootTimer.IsStarted())
        {
            shootTimer.StartTimer(delay, 0, "ShootIt");
        }
    }
    public void Zoom(CrossHair crossHair)
    {
        if(scopeTexture != null)
            crossHair.SetCrosshair(scopeTexture, scopeTextureSize);

        Player player = holder as Player;
        if (player != null)
        {
            player.mouseLook.sensitivityX = 5;
            player.mouseLook.sensitivityY = 5;
        }
        Camera.main.fieldOfView = Camera.main.fieldOfView - fieldOfViewZoom;
    }
    public void Reload()
    {
        if (!reloadTimer.IsStarted())
        {
            reloadTimer.StartTimer(reloadDelay, 0, "ReloadIt");
             if(reloadSound != null)
                AudioSource.PlayClipAtPoint(reloadSound, this.gameObject.transform.position);
        }
    }

    private void ReloadIt()
    {
        if (bullets > 0)
        {
            bullets += bulletsInMeg;
            if(bullets > maxMegazineSize)
            {
                bulletsInMeg = maxMegazineSize;
                bullets -= bulletsInMeg;
                megazine = bullets / maxMegazineSize;//bulletsInMeg % bullets;
            }
            else
            {
                bulletsInMeg = bullets;
                bullets = 0;
                megazine = 0;
            }
        }
    }
    private void ShootIt()
    {
        canFire = true;
    }
}
