using UnityEngine;

public class ShibaSlimeAbility : SlimeAbility
{
    [SerializeField]
    private GameObject m_digObject = null;

    [SerializeField]
    private int m_minDig = 3;

    [SerializeField]
    private int m_maxDig = 8;

    protected override void ActiveActive()
    {
        GameManager.Instance.AudioManager.ActiveAbilitySuccess();

        int digAmount = Random.Range(m_minDig, m_maxDig + 1);

        for(int i = 0; i < digAmount; ++i)
        {
            GameObject dug = Instantiate(m_digObject, GameManager.Instance.transform);
            dug.transform.position = gameObject.transform.position;
        }
    }

    protected override void UpdatePassive(float deltaTime)
    {
    }
}
