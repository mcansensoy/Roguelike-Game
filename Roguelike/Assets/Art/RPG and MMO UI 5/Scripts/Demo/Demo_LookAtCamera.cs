using UnityEngine;

namespace DuloGames.UI
{
    public class Demo_LookAtCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera m_Camera;

        protected void Awake()
        {
            if (this.m_Camera == null) this.m_Camera = UnityEngine.Camera.main;
        }

        void Update()
        {
            if (this.m_Camera)
                transform.rotation = Quaternion.LookRotation(this.m_Camera.transform.forward);
        }
    }
}
