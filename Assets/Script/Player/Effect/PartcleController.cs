using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;


public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem fallParticle;
    [SerializeField] ParticleSystem wallParticle;

    [Range(0, 10)] 
    [SerializeField]private int occurAfterVelocity;
    [Range(0,0.2f)]
    [SerializeField]private float dustFormationPeriod;

   private float counter;
   private bool wasInAir = false;
   
   

   private void Update()
   {
       counter += Time.deltaTime;
       if (PlayerManager.instance.player.IsGroundDetected() && Math.Abs(PlayerManager.instance.player.rb.linearVelocity.x) > occurAfterVelocity)
       {
           
           movementParticle.Play();
           counter = 0;
           
       }

       if (!wasInAir && PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.airState)
       {
           Debug.Log("in air");
           wasInAir = true;
       }

       if (wasInAir && PlayerManager.instance.player.IsGroundDetected() &&
           PlayerManager.instance.player.rb.linearVelocity.y <= 0)
       {
           
           fallParticle.Play();
           wasInAir = false;
       }
   }
}