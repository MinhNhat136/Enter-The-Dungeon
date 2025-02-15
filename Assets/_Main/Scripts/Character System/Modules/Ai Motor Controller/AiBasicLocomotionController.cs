﻿using System;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace Atomic.Character
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Controls locomotion for AI agents using a NavMeshAgent for navigation. 
    /// Note that the associated Animator component should utilize root motion.
    /// CAUTION: USING THIS CLASS WITH AnimatorEventsListenerWithRootMotion IN CHILD GAME OBJECT CONTAIN ANIMATOR.
    /// </summary>
    public class AiBasicLocomotionController : ILocomotionController
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized => _isInitialized;

        public AiMotorController Model => _model;

        public bool IsNavMeshRotate { get; set; }
        //  Fields ----------------------------------------
        private NavMeshAgent _navMeshAgent;
        private AiMotorController _model;
        private Animator _animator;

        private bool _isInitialized;
        private bool _isStopped;
        
        //  Initialization  -------------------------------
        public void Initialize(AiMotorController model)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _model = model;

                _navMeshAgent = _model.BaseNavMeshAgent;
                _animator = _model.BaseAnimator;

                _animator.applyRootMotion = false;
                _navMeshAgent.updatePosition = true;
                _navMeshAgent.updateRotation = IsNavMeshRotate;
            }
        }

        public void RequireIsInitialized()
        {

            if (!_isInitialized)
            {
                throw new System.Exception("Not initialized player locomotion system");
            }
        }

        //  Unity Methods   -------------------------------
        

        //  Other Methods ---------------------------------
        public void ApplyRotation()
        {
            if (IsNavMeshRotate)
            {
                // Rotate using NavMeshAgent
                if (_navMeshAgent.desiredVelocity != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(_navMeshAgent.desiredVelocity);
                    _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, Model.RotationSpeed * Time.deltaTime);
                }
                return;
            }
            if (_model.MoveDirection == Vector3.zero)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(_model.MoveDirection);

            desiredRotation.x = 0;
            desiredRotation.z = 0;

            _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, desiredRotation, Model.RotationSpeed * Time.deltaTime);
        }
        
        public void ApplyMovement()
        {
            if (_isStopped) _isStopped = false;
            Vector3 destination = _model.transform.position + _model.MoveDirection;
            _navMeshAgent.SetDestination(destination);
        }

        public void ApplyStop()
        {
            if (_isStopped) 
                return;
            _model.MoveInput = Vector2.zero;
            _model.MoveDirection = Vector3.zero;
            _navMeshAgent.destination = _model.transform.position;
            _isStopped = true;
        }
    }

}
