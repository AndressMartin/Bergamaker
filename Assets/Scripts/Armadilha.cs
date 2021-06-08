using UnityEngine;

public class Armadilha : MonoBehaviour
{
    int Dano = -10;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "Player" /*|| collision.tag == "Enemy"*/)
            {
                if (!collision.transform.parent.GetComponent<Dash>().dashing)
                    CausarDano(collision.transform.parent);
            }
        }
    }

    private void CausarDano(Transform collider)
    {
        var player = collider.GetComponent<Player>();
        Debug.Log($"AIIIIIIIIIIIIIIIIIIII {collider}");
        player.AlterarPV(Dano);
    }
}
