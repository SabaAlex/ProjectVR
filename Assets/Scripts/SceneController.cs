using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DigitalRuby.RainMaker;
using DigitalRuby.LightningBolt;

public class SceneController : MonoBehaviour
{
    public GameObject rain;
    public GameObject sun;
    public GameObject[] lightningBolts;
    public AudioClip[] thunderSounds;
    public GameObject tornado;
    public GameObject[] tornadoPoints;
    public GameObject cabin;
    public GameObject[] cabinPoints;
    public Image blackScreen;

    // Elapsed time
    private float time;
        
    // References to component scripts
    private RainScript rainScript;

    // Control variables
    private float lightningInterval;

    private int tornadoPointIndex;
    private Transform tornadoTarget;

    private int cabinPointIndex;
    private Transform cabinTarget;

    private bool fadedToBlack;

    void Start()
    {
        time = 0.0f;

        rainScript = rain.GetComponent<RainScript>();

        lightningInterval = 0.0f;

        tornadoPointIndex = 0;
        tornadoTarget = tornadoPoints[0].transform;

        cabinPointIndex = 0;
        cabinTarget = cabinPoints[0].transform;

        blackScreen.color = Color.black;
        blackScreen.canvasRenderer.SetAlpha(0.0f);

        fadedToBlack = false;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 10.0f)
        {
            // Starting the rain.
            if (rainScript.RainIntensity + Time.deltaTime * 0.05f <= 1.0f)
            {
                rainScript.RainIntensity += Time.deltaTime * 0.05f;
                rainScript.RainMistThreshold += Time.deltaTime * 0.05f;
                rainScript.WindSoundVolumeModifier += Time.deltaTime * 0.25f;
            }

            // Turning to dusk.
            Quaternion target = Quaternion.Euler(-120.0f, 0, 0);
            sun.transform.rotation = Quaternion.RotateTowards(sun.transform.rotation, target, Time.deltaTime * 2.5f);

            // Adding lightning bolts.
            lightningInterval += Time.deltaTime;

            if (lightningInterval >= Random.Range(10.0f, 15.0f))
            {
                // Trigger lightning bolts.
                GameObject lastLightningBolt = null;

                foreach (GameObject lightningBolt in lightningBolts)
                {
                    LightningBoltScript lightningBoltScript = lightningBolt.GetComponent<LightningBoltScript>();
                    lightningBoltScript.Trigger();

                    lastLightningBolt = lightningBolt;
                }

                // Play a thunder sound.
                int thunderSoundIndex = Random.Range(0, thunderSounds.Length);
                AudioClip thunderSound = thunderSounds[thunderSoundIndex];

                AudioSource audioSource = lastLightningBolt.GetComponent<AudioSource>();

                audioSource.clip = thunderSound;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                // Reset the lightning interval.
                lightningInterval = 0.0f;
            }

            // Adding Tzanca Hurricane
            if (time >= 30.0f)
            {
                // Call the particle system Play function.
                ParticleSystem tornadoParticleSystem = tornado.GetComponent<ParticleSystem>();
                if (!tornadoParticleSystem.isPlaying)
                { 
                    tornadoParticleSystem.Play();
                }

                // Move to the next target point.
                tornado.transform.position = Vector3.MoveTowards(tornado.transform.position, tornadoTarget.position, Time.deltaTime * 5.0f);

                // Check if the position of the tornado and the next point are approximately equal.
                if (Vector3.Distance(tornado.transform.position, tornadoTarget.position) < 0.001f)
                {
                    // Set the next target position.
                    if (tornadoPointIndex < tornadoPoints.Length - 1)
                    {
                        tornadoPointIndex += 1;
                        tornadoTarget = tornadoPoints[tornadoPointIndex].transform;
                    } else
                    {
                        tornadoTarget = cabin.transform;

                        // Start moving the cabin.

                        // Move to the next target point.
                        cabin.transform.position = Vector3.MoveTowards(cabin.transform.position, cabinTarget.position, Time.deltaTime * 5.0f);

                        // And do some rotation
                        cabin.transform.rotation = Quaternion.RotateTowards(cabin.transform.rotation, cabinTarget.rotation, Time.deltaTime * 5.0f);

                        // Check if the position of the cabin and the next point are approximately equal.
                        if (Vector3.Distance(cabin.transform.position, cabinTarget.position) < 0.001f)
                        {
                            // Set the next target position.
                            if (cabinPointIndex < cabinPoints.Length - 1)
                            {
                                cabinPointIndex += 1;
                                cabinTarget = cabinPoints[cabinPointIndex].transform;
                            }

                            // We're at the end, fade to black.
                            if (cabinPointIndex == cabinPoints.Length - 1 && !fadedToBlack)
                            {
                                blackScreen.CrossFadeAlpha(1.0f, 2.0f, false);
                                fadedToBlack = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
