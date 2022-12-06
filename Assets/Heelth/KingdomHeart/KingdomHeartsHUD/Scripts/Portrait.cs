using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    [SerializeField]
    Image normalPortrait;
    [SerializeField]
    Image damagedPortrait;
    [SerializeField]
    float damageDisplayTime = 1;

    [SerializeField]
    private Gradient damageGradient;

    float timer;

    public void DisplayDamagedFace()
    {
        timer = damageDisplayTime;
    }

    public void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0;
            }
            damagedPortrait.color = damageGradient.Evaluate(1-(timer/damageDisplayTime));
        }
    }

    public void OnReceiveDamage(int value)
    {
        this.DisplayDamagedFace();
        //this.animator.SetTrigger("Damage");

        if (value <= 50)
        {
            //audioSource.PlayOneShot(damageSounds[0]);
        }
        else if (value <= 100)
        {
            //audioSource.PlayOneShot(damageSounds[1]);
        }
        else
        {
            //audioSource.PlayOneShot(damageSounds[2]);
        }
        //int rand = Random.Range(0, damageSounds.Length);
    }

}