using UnityEngine;

public class Fish : MonoBehaviour   // Add fish class to each fish to tell fishes to swim, Unity dosen't have any water physics built in
{
    public float fishSpeed; // The speed of the fishes
    private float randomizedSpeed = 0f; // Slightly altered speed changed by fishe's position
    private float nextActionTime = -1f; // Trigger selection of a new swim destination
    private Vector3 targetPosition; // Position of the destination the fish is swimming for

    private void FixedUpdate()  // This is something which updates every .02 seconds regardless of frame rate, so good for ML Agents
    {
        if (fishSpeed > 0)
        {
            Swim();
        }
    }

    private void Swim()
    {
        // If it's time for the next action, pick a new speed and destination
        // Else, swim toward the destination
        if (Time.fixedTime >= nextActionTime)
        {
            // Randomize the speed
            randomizedSpeed = fishSpeed * UnityEngine.Random.Range(.5f, 1.5f);

            // Pick a random target
            targetPosition = PenguinArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);

            // Rotate toward the target
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

            // Calculate the time to get there
            float timeToGetThere = Vector3.Distance(transform.position, targetPosition) / randomizedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            // Make sure that the fish does not swim past the target
            Vector3 moveVector = randomizedSpeed * transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(transform.position, targetPosition))
            {
                transform.position += moveVector;
            }
            else
            {
                transform.position = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }
}