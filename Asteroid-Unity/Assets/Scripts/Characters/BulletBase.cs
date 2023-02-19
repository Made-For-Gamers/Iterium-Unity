using UnityEngine;

namespace Iterium
{
    // Bullet base class that handles...
    // * Bullet explosion

    public abstract class BulletBase : MonoBehaviour, IDamage
    {
        //Release bullet after leaving the screen
        private void OnBecameInvisible()
        {
            ReleaseBullet();
        }

        public void Damage(float firePower, string attacker)
        {
            ReleaseBullet();
        }

        //Explosion
        protected void BulletExplosion(Collider obj)
        {
            SoundManager.Instance.PlayEffect(1);
            GameObject explosionObject = ExplosionPooling.explosionPool.Get();
            explosionObject.transform.position = obj.transform.position;
            explosionObject.transform.rotation = obj.transform.rotation;
            ReleaseBullet();
        }

        //Overriden by class children
        protected virtual void ReleaseBullet() { }
    }
}