using UnityEngine;
using System.Collections;

public partial class PlayerMotion : MonoBehaviour
{
    public IEnumerator StandingLightAttack()
    {
        _StandingLightAttack.SetActive(true); 
        /*var _GameObject = new GameObject();
        _GameObject.tag = "Attacks/StandingAttacks/StandingLightAttack";
        _GameObject.transform.parent = transform;
        _GameObject.transform.position = transform.position;

        var StandingLightAttackBounds = _GameObject.AddComponent<BoxCollider2D>();
        StandingLightAttackBounds.isTrigger = true;*/

        if (PlayerState.FacingRight.Value)
        {
            /*StandingLightAttackBounds.offset = new Vector2(0.1095207f, 0.03352177f);
            StandingLightAttackBounds.size = new Vector2(0.2048863f, 0.04506044f);*/
            _StandingLightAttack.GetComponent<BoxCollider2D>().offset = new Vector2(_StandingLightAttack.GetComponent<BoxCollider2D>().offset.x, _StandingLightAttack.GetComponent<BoxCollider2D>().offset.y);
            _StandingLightAttack.GetComponent<BoxCollider2D>().size = new Vector2(_StandingLightAttack.GetComponent<BoxCollider2D>().size.x, _StandingLightAttack.GetComponent<BoxCollider2D>().size.y);
        }
        else
        {
            _StandingLightAttack.GetComponent<BoxCollider2D>().offset = new Vector2(_StandingLightAttack.GetComponent<BoxCollider2D>().offset.x * -1f, _StandingLightAttack.GetComponent<BoxCollider2D>().offset.y);
            _StandingLightAttack.GetComponent<BoxCollider2D>().size = new Vector2(_StandingLightAttack.GetComponent<BoxCollider2D>().size.x, _StandingLightAttack.GetComponent<BoxCollider2D>().size.y);
        }

        for(var i = 0;i < 3;i++)
        {
            yield return null;
        }

        _StandingLightAttack.SetActive(false);
    }
}
