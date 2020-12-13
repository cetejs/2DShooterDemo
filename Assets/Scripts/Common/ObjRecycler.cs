using System.Collections;
using UnityEngine;

namespace Common
{
    public class ObjRecycler : MonoBehaviour
    {
        [Header("持续多长时间就会被回收，当为零时表示不可回收")]
        public float duration = 1;

        private WaitForSeconds m_WaitForSecondsToRecovery;

        public void RecycleObj(string prefName = null)
        {
            if (prefName == null || gameObject.name.Contains(prefName))
            {
                ObjPoolMgr.Instance.RecycleObj(gameObject);
            }
        }

        protected virtual void Awake()
        {
            m_WaitForSecondsToRecovery = new WaitForSeconds(duration);
        }

        private void OnEnable()
        {
            GameMgr.Instance.OnNeedRecycleAllCommonObjs += RecycleObj;

            if (duration == 0)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(Recovery());
        }

        private void OnDisable()
        {
            GameMgr.Instance.OnNeedRecycleAllCommonObjs -= RecycleObj;
        }

        private IEnumerator Recovery()
        {
            yield return m_WaitForSecondsToRecovery;
            ObjPoolMgr.Instance.RecycleObj(gameObject);
        }
    }
}