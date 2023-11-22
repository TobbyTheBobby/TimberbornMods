using System;
using UnityEngine;
using Timberborn.Animations;
using Timberborn.MechanicalSystem;
using Timberborn.TickSystem;

namespace VerticalPowerShaft
{
  public class ShaftAnimator : TickableComponent
  {
    private float _speed;
    private IAnimator _activeAnimator;
    private Animator _animator;
    private MechanicalNode _mechanicalNode;
    private ITransputSpecificationsProvider _transputSpecificationsProvider;
    public bool ReversedRotation { get; private set; }

    public void Awake()
    {
      _activeAnimator = GetComponentInChildren<IAnimator>(false);
      _animator = GetComponentInChildren<Animator>();
      _mechanicalNode = GetComponentFast<MechanicalNode>();
      _transputSpecificationsProvider = GetComponentFast<ITransputSpecificationsProvider>();
    }

    public override void Tick()
    {
      if (_activeAnimator == null) return;
      if (!CanAnimate())
      {
        _activeAnimator.Enabled = false;
        return;
      }

      _activeAnimator.Enabled = true;
      UpdateDirection();
      UpdateAnimation();
    }

    private void UpdateAnimation()
    {
      try
      {
        _speed = ReversedRotation ? -1f : 1f;
        _activeAnimator.Enabled = true;
        _animator.SetFloat("SpeedParameter", _speed);
      }
      catch (Exception e)
      {
        Plugin.Log.LogError($"Animation Error: {e}");
      }
    }
    
    private void UpdateDirection()
    {
      ReversedRotation = _mechanicalNode.Transputs[0].ReversedRotation != _transputSpecificationsProvider.GetTransputSpecifications()[0].ReverseRotation;
    }
    
    private bool CanAnimate()
    {
      if (!_mechanicalNode.ActiveAndPowered)
        return false;
      return _mechanicalNode.IsConsuming || _mechanicalNode.IsShaft || _mechanicalNode.IsGenerator;
    }
  }
  
    // _____ OLD CODE _____
    
    // public class ShaftAnimator : TickableComponent, IFinishedStateListener, IPreviewStateListener, IUnfinishedStateListener
    // {
    //   private NonlinearAnimationManager _nonlinearAnimationManager;
    //   private MechanicalNode _mechanicalNode;
    //   private ITransputSpecificationsProvider _transputSpecificationsProvider;
    //   private IAnimator[] _animators;
    //   private IAnimator _activeAnimator;
    //   private float _currentAnimationSpeed;
    //
    //   [Inject]
    //   public void InjectDependencies(
    //     NonlinearAnimationManager nonlinearAnimationManager)
    //   {
    //     this._nonlinearAnimationManager = nonlinearAnimationManager;
    //   }
    //
    //   public void Awake()
    //   {
    //     this._mechanicalNode = this.GetComponentFast<MechanicalNode>();
    //     this._transputSpecificationsProvider = this.GetComponentFast<ITransputSpecificationsProvider>();
    //     this._animators = this.GetComponentsInChildren<IAnimator>(true);
    //     this.enabled = false;
    //   }
    //
    //   public override void StartTickable() => this.FindAndUpdateActiveAnimator();
    //
    //   public override void Tick() => this.UpdateAnimation();
    //
    //   public void OnEnterFinishedState() => this.enabled = true;
    //
    //   public void OnExitFinishedState() => this.enabled = false;
    //
    //   public void OnEnterPreviewState() => this.StopAnimators();
    //
    //   public void OnEnterUnfinishedState() => this.StopAnimators();
    //
    //   public void OnExitUnfinishedState()
    //   {
    //   }
    //
    //   private void StopAnimators()
    //   {
    //     foreach (IAnimator animator in this._animators)
    //       animator.Enabled = false;
    //   }
    //
    //   private void FindAndUpdateActiveAnimator()
    //   {
    //     this._activeAnimator = this.GetComponentInChildren<IAnimator>(false);
    //     this.StopAnimators();
    //     this.UpdateAnimation();
    //   }
    //
    //   private void UpdateAnimation()
    //   {
    //     if (this._activeAnimator == null)
    //       return;
    //     if (this.CanAnimate())
    //     {
    //       bool reversedRotation = this._mechanicalNode.Transputs[0].ReversedRotation != this._transputSpecificationsProvider.GetTransputSpecifications()[0].ReverseRotation;
    //       int num = reversedRotation ? -1 : 1;
    //       this._activeAnimator.Enabled = true;
    //       this._activeAnimator.Speed = this._mechanicalNode.PowerEfficiency * this._nonlinearAnimationManager.SpeedMultiplier * (float) num;
    //     }
    //     else
    //       this._activeAnimator.Enabled = false;
    //   }
    //
    //   private bool CanAnimate()
    //   {
    //     if (!this._mechanicalNode.ActiveAndPowered)
    //       return false;
    //     return this._mechanicalNode.IsConsuming || this._mechanicalNode.IsShaft || this._mechanicalNode.IsGenerator;
    //   }
    // }
}