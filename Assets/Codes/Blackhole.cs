using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : Common
{

    public AnimationCurve legCurve;

    private float aniTime = 0f;
    private int setp = 0;
    private float[] setpTims = { 3f, 0.5f };

    private void FixedUpdate()
    {
        switch (setp)
        {
            case 0:
                {
                    Vector3 s1 = ScaleTransform(Vector3.one / 10f, new Vector3(3f, 3f, 1f), aniTime, setpTims[setp], legCurve);
                    Vector3 s2 = ScaleTransform(new Vector3(0.1f, 0.1f, 0f), Vector3.zero, aniTime, setpTims[setp], legCurve);
                    Vector3 s3 = LoopScaleTransform(Vector3.one - s2, Vector3.one + s2, aniTime, 0.05f);

                    transform.localScale = new Vector3(s1.x * s3.x, s1.y * s3.y, 1f);

                    if(aniTime > setpTims[setp])
                    {
                        aniTime = 0f;
                        setp++;
                    }
                }
                break;
            case 1:
                {
                    Vector3 s1 = ScaleTransform(new Vector3(3f, 3f, 1f), Vector3.one / 10f, aniTime, setpTims[setp], legCurve);

                    transform.localScale = s1;

                    if (aniTime > setpTims[setp])
                    {
                        aniTime = 0f;
                        setp++;
                    }
                }
                break;
            default:
                {
                    Destroy(gameObject);
                }
                break;
        }

        aniTime += Time.fixedDeltaTime;

    }

}
