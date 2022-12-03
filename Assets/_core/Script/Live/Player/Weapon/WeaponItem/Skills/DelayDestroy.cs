using Script.Abstract;
using Script.AssetFactory;

public class DelayDestroy : PoolizeGBDefault
{
    public float delay = 3;

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        Invoke("DestroyEffect", delay);
    }

    void DestroyEffect()
    {
        GameFacade.Instance.GetInstance<IGameObjectPool>().Enqueue(gameObject);
    }
}