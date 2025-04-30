using UnityEngine;

public class PolyProjectile : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController morphAnimator;
    [SerializeField] GameObject morphWeapon;
    [SerializeField] GameObject morphSecondary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyMorph(GameObject player)
    {
        Debug.Log("Player Morph Time");
        Polymorph p = player.AddComponent<Polymorph>();
        p.SetupMorph(morphAnimator, morphWeapon, morphSecondary);
    }
}
