namespace Iterium
{
    //Interface for all gameObjects that take damage...
    //Asteroids, Player, AI, NPC, Bullets

    public interface IDamage 
    {
        public void Damage(float firePower, string attacker);
    }

}
