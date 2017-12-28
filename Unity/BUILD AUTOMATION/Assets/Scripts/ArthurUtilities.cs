using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Arthur.Client.Utilities
{
    public static class ArthurUtilities
    {
        static ArthurBuildSettings arthurBuildSettings;

        public static ArthurBuildSettings ArthurBuildSettings
        {
            get
            {
                if (arthurBuildSettings == null)
                {
                    arthurBuildSettings = (ArthurBuildSettings)Resources.Load("Arthur/ArthurBuildSettings");
                }
                return arthurBuildSettings;
            }
        }

        public static Color HsvToColor(float pHue, float pSat, float pVal)
        {
            float hue60 = pHue / 60f;
            int i = (int)Mathf.Floor(hue60) % 6;
            float f = hue60 - (int)Mathf.Floor(hue60);

            float v = pVal;
            float p = pVal * (1 - pSat);
            float q = pVal * (1 - f * pSat);
            float t = pVal * (1 - (1 - f) * pSat);

            switch (i)
            {
                case 0: return new Color(v, t, p);
                case 1: return new Color(q, v, p);
                case 2: return new Color(p, v, t);
                case 3: return new Color(p, q, v);
                case 4: return new Color(t, p, v);
                case 5: return new Color(v, p, q);
            }

            return Color.black;
        }

        public static byte[] ArrayFromColor32(Color32 c)
        {
            byte[] retVal = new byte[4];
            retVal[0] = c.r;
            retVal[1] = c.g;
            retVal[2] = c.b;
            retVal[3] = c.a;

            return retVal;
        }

        public static Color32 Color32FromByteArray(byte[] array)
        {
            if (array == null)
                return Color.white;

            Color32 retVal = new Color32();
            retVal.r = array[0];
            retVal.g = array[1];
            retVal.b = array[2];
            retVal.a = array[3];
            return retVal;
        }

        public static bool Color32Equals(Color32 a, Color32 b)
        {
            return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        }

        public static void AppendAllBytes(string path, byte[] bytes)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void AppendAllBytesWithPCMHeader(string path, float[] data, int freq, int channels, int samples)
        {
            //argument-checking here.


            /*
            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
                Debug.Log("Bytes: " + bytes);
               // SavWav.WriteHeader(stream, freq,channels,samples);
            }
            */


            
            

        }
        
        

      

        public static string ToLongString(this Vector3 vec)
        {
            return (vec.x + ", " + vec.y + ", " + vec.z);
        }

        public static bool MaskContainsLayer(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static string PrintVector3(Vector3 vec)
        {
            return vec.x + "," + vec.y + "," + vec.z;
        }

        public static float GetRadius(Bounds bounds)
        {
            float radius = bounds.extents.x;
            if (bounds.extents.y > radius)
            {
                radius = bounds.extents.y;
                if (bounds.extents.z > radius)
                    radius = bounds.extents.z;
            }
            else
            {
                if (bounds.extents.z > radius)
                    radius = bounds.extents.z;
            }

            return radius;
        }

        public static Bounds getBounds(GameObject gameObject)
        {
            Bounds bounds;
            Renderer childRender;
            bounds = getRenderBounds(gameObject);
            if (bounds.extents.x == 0)
            {
                bounds = new Bounds(gameObject.transform.position, Vector3.zero);
                foreach (Transform child in gameObject.transform)
                {
                    childRender = child.GetComponent<Renderer>();
                    if (childRender)
                    {
                        bounds.Encapsulate(childRender.bounds);
                    }
                    else
                    {
                        bounds.Encapsulate(getBounds(child.gameObject));
                    }
                }
            }
            return bounds;
        }

        public static Bounds getRenderBounds(GameObject gameObject)
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            Renderer render = gameObject.GetComponent<Renderer>();
            if (render != null)
            {
                return render.bounds;
            }
            return bounds;
        }

        public static Texture2D AlphaBlend(this Texture2D aBottom, Texture2D aTop)
        {
            //Debug.Log("aBottom.width: " + aBottom.width+ " aTop.width: "+ aTop.width + " aBottom.height:"+ aBottom.height + " aTop.height:"+ aTop.height );
            if (aBottom.width != aTop.width || aBottom.height != aTop.height)
                throw new System.InvalidOperationException("AlphaBlend only works with two equal sized images");
            var bData = aBottom.GetPixels();
            var tData = aTop.GetPixels();
            int count = bData.Length;
            var rData = new Color[count];
            for (int i = 0; i < count; i++)
            {
                Color B = bData[i];
                Color T = tData[i];
                float srcF = T.a;
                float destF = 1f - T.a;
                float alpha = srcF + destF * B.a;
                Color R = (T * srcF + B * B.a * destF) / alpha;
                R.a = alpha;
                rData[i] = R;
            }
            var res = new Texture2D(aTop.width, aTop.height);
            res.SetPixels(rData);
            res.Apply();
            return res;
        }

    }

    public static class TextureScale
    {
        public class ThreadData
        {
            public int start;
            public int end;
            public ThreadData(int s, int e)
            {
                start = s;
                end = e;
            }
        }

        private static Color[] texColors;
        private static Color[] newColors;
        private static int w;
        private static float ratioX;
        private static float ratioY;
        private static int w2;
        private static int finishCount;
        private static Mutex mutex;

        public static void Point(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, false);
        }

        public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, true);
        }

        private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
        {
            texColors = tex.GetPixels();
            newColors = new Color[newWidth * newHeight];
            if (useBilinear)
            {
                ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
                ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
            }
            else
            {
                ratioX = ((float)tex.width) / newWidth;
                ratioY = ((float)tex.height) / newHeight;
            }
            w = tex.width;
            w2 = newWidth;
            var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
            var slice = newHeight / cores;

            finishCount = 0;
            if (mutex == null)
            {
                mutex = new Mutex(false);
            }
            if (cores > 1)
            {
                int i = 0;
                ThreadData threadData;
                for (i = 0; i < cores - 1; i++)
                {
                    threadData = new ThreadData(slice * i, slice * (i + 1));
                    ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                    Thread thread = new Thread(ts);
                    thread.Start(threadData);
                }
                threadData = new ThreadData(slice * i, newHeight);
                if (useBilinear)
                {
                    BilinearScale(threadData);
                }
                else
                {
                    PointScale(threadData);
                }
                while (finishCount < cores)
                {
                    Thread.Sleep(1);
                }
            }
            else
            {
                ThreadData threadData = new ThreadData(0, newHeight);
                if (useBilinear)
                {
                    BilinearScale(threadData);
                }
                else
                {
                    PointScale(threadData);
                }
            }

            tex.Resize(newWidth, newHeight);
            tex.SetPixels(newColors);
            tex.Apply();

            texColors = null;
            newColors = null;
        }

        public static void BilinearScale(System.Object obj)
        {
            ThreadData threadData = (ThreadData)obj;
            for (var y = threadData.start; y < threadData.end; y++)
            {
                int yFloor = (int)Mathf.Floor(y * ratioY);
                var y1 = yFloor * w;
                var y2 = (yFloor + 1) * w;
                var yw = y * w2;

                for (var x = 0; x < w2; x++)
                {
                    int xFloor = (int)Mathf.Floor(x * ratioX);
                    var xLerp = x * ratioX - xFloor;
                    newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                           ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                           y * ratioY - yFloor);
                }
            }

            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        public static void PointScale(System.Object obj)
        {
            ThreadData threadData = (ThreadData)obj;
            for (var y = threadData.start; y < threadData.end; y++)
            {
                var thisY = (int)(ratioY * y) * w;
                var yw = y * w2;
                for (var x = 0; x < w2; x++)
                {
                    newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
                }
            }

            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value,
                              c1.g + (c2.g - c1.g) * value,
                              c1.b + (c2.b - c1.b) * value,
                              c1.a + (c2.a - c1.a) * value);
        }
    }
}
