using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBibleSample
{
    public class FollowCamera : NetworkBehaviour
    {
        [SerializeField] private Transform m_FollowCamera;

        [SerializeField][Networked] private Quaternion quaternion { get; set; }
        private Interpolator<Quaternion> quaternionInterpolator;


        void Start()
        {
            if (Object.HasInputAuthority == false) return;

            quaternion = Quaternion.identity;
            quaternionInterpolator = GetInterpolator<Quaternion>(nameof(quaternion));

            m_FollowCamera = GameObject.FindGameObjectsWithTag("MainCamera").Where(x => x.scene == gameObject.scene).First().transform;

            
            //transform.position = m_FollowCamera.position;
            //transform.rotation = m_FollowCamera.rotation;
        }

        public override void FixedUpdateNetwork()
        {
            if (m_FollowCamera == null) return;

            quaternion = m_FollowCamera.transform.localRotation;
        }

        public override void Render()
        {
            if (m_FollowCamera == null) return;

            transform.position = m_FollowCamera.position;
            transform.rotation = quaternionInterpolator.Value;

            //transform.position = m_FollowCamera.position;
            //transform.rotation = m_FollowCamera.rotation;
        }
    }
}