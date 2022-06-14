using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public int colorMaterialIndex;

    public Species species;
    public Material material;

    public Coord coord;

    [HideInInspector]
    public int mapIndex;
    [HideInInspector]
    public Coord mapCoord;

    protected bool dead;

    public virtual void Init(Coord coord)
    {
        this.coord = coord;

        transform.position = Environment.tileCentres[coord.x, coord.y];

        var meshRenderer = transform.GetComponentInChildren<MeshRenderer>();

        for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
        {
            if(meshRenderer.sharedMaterials[i] == material)
            {
                material = meshRenderer.materials[i];

                break;
            }
        }
    }

    public virtual void Die(CauseOfDeath cause)
    {
        if(!dead)
        {
            dead = true;
            Environment.RegisterDeath(this);
            Destroy(gameObject);
        }
    }
}
