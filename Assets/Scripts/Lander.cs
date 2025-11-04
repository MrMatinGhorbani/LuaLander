using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    public static Lander instance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickedUp;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State stateEvent;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public float landingSpeed;
        public float dotVector;
        public float scoreMultiplier;
        public int score;
    }
    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    private Rigidbody2D landerRigidbody2d;
    private float maxFuelAmount = 10f;
    private float fuelAmount;
    private State state;
    private void Awake()
    {
        instance = this;
        fuelAmount = maxFuelAmount;
        state = State.WaitingToStart;
        landerRigidbody2d = GetComponent<Rigidbody2D>();
        landerRigidbody2d.gravityScale = 0f;

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        // Debug.Log("Fuel" + fuelAmount);
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (GameInput.instance.IsUpActionPressed() ||
                    GameInput.instance.IsRightActionPressed() ||
                    GameInput.instance.IsLeftActionPressed()||
                    GameInput.instance.GetMovementInputVector2()!=Vector2.zero)
                {
                    //Press Any Input
                    landerRigidbody2d.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);


                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f)
                {
                    fuelAmount = 0f;
                    return;
                }
                if (GameInput.instance.IsUpActionPressed() ||
                    GameInput.instance.IsRightActionPressed() ||
                    GameInput.instance.IsLeftActionPressed()||
                      GameInput.instance.GetMovementInputVector2() != Vector2.zero)
                {
                    ConsumeFuel();
                }
                float gamepadDeadzone = .4f;
                if (GameInput.instance.IsUpActionPressed() || GameInput.instance.GetMovementInputVector2().y>gamepadDeadzone)
                {
                    float force = 700f;
                    landerRigidbody2d.AddForce(transform.up * force * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);

                }
                if (GameInput.instance.IsLeftActionPressed()|| GameInput.instance.GetMovementInputVector2().x < -gamepadDeadzone)
                {
                    float turnSpeed = +100f;
                    landerRigidbody2d.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.instance.IsRightActionPressed() || GameInput.instance.GetMovementInputVector2().x > gamepadDeadzone)
                {
                    float turnSpeed = -100f;
                    landerRigidbody2d.AddTorque(turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Landed on Terrain!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        float softLandingMagnitude = 4f;
        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingMagnitude)
        {   //Landed too hard!
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            //Landed on a too steep angle!
            Debug.Log("Landed on a too steep angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;
        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("LandingAngleScore is :" + landingAngleScore);
        Debug.Log("LandingSpeedScore is :" + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        Debug.Log("Score :" + score);

        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FuelPickUp fuelPickUp))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > maxFuelAmount)
            {
                fuelAmount = maxFuelAmount;
            }
            fuelPickUp.DestroySelf();
        }
        if (collision.gameObject.TryGetComponent(out CoinPickUp coinPickUp))
        {
            OnCoinPickedUp?.Invoke(this, EventArgs.Empty);
            coinPickUp.DestroySelf();
        }
    }
    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            stateEvent = state,
        });
    }
    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }
    public float GetSpeedX()
    {
        return landerRigidbody2d.velocity.x;
    }
    public float GetSpeedY()
    {
        return landerRigidbody2d.velocity.y;
    }
    public float GetFuel()
    {
        return fuelAmount;
    }
    public float GetNormalizedFuel()
    {
        return fuelAmount / maxFuelAmount;
    }
}
