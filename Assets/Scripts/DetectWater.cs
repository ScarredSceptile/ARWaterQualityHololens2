using OpenCvSharp;
using System.Threading.Tasks;
using UnityEngine;

public class DetectWater : MonoBehaviour
{
    // Standard color: 255, 218, 170
    Scalar a = new Scalar(255, 228, 180);
    Scalar b = new Scalar(245, 208, 160);
    public Texture2D mapImg;
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        // plane = object
        mapImg = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        renderer = GetComponent<Renderer>();
        Create();
    }

    //void Update()
    void Create()
    {
        Mat frame, mask;
        Mat canny_output;
        mask = new Mat();
        canny_output = new Mat();
        Point[][] contours;
        HierarchyIndex[] hierarchy;

        frame = Tex2DToMat(mapImg);
        Cv2.InRange(InputArray.Create(frame), b, a, mask);

        Cv2.Canny(mask, canny_output, 255, 100);

        Cv2.FindContours(canny_output, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

        Mat drawing = Mat.Zeros(canny_output.Size(), MatType.CV_8UC1);
        Debug.Log(contours.Length);
        for (int i = 0; i < contours.Length; i++)
        {
            Cv2.DrawContours(drawing, contours, i, Scalar.Aquamarine, 1, LineTypes.Link8, hierarchy, 0);
        }

        var newMap = MatToTex2D(drawing);
        renderer.material.mainTexture = newMap;
    }

    private Mat Tex2DToMat(Texture2D source)
    {

        int imgHeight = source.height;
        int imgWidth = source.width;

        Vec3d[] data = new Vec3d[imgHeight * imgWidth];

        var c = source.GetPixels32();

        Parallel.For(0, imgHeight, i =>
        {
            for (int j = 0; j < imgWidth; j++)
            {
                var col = c[j + i * imgWidth];
                var vec = new Vec3d
                {
                    Item0 = col.b,
                    Item1 = col.g,
                    Item2 = col.r
                };
                data[j + i * imgWidth] = vec;
            }
        });

        Mat mat = new Mat(imgWidth, imgHeight, MatType.CV_64FC3, data);
        mat = mat.Flip(FlipMode.X);
        mat.ConvertTo(mat, MatType.CV_8UC3);
        return mat;
    }

    private Texture2D MatToTex2D(Mat source)
    {
        int imgHeight = source.Height;
        int imgWidth = source.Width;

        byte[] data = new byte[imgHeight * imgWidth];
        source.GetArray(0, 0, data);

        Color32[] colors = new Color32[imgHeight * imgWidth];

        Parallel.For(0, imgHeight, i =>
        {
            for (var j = 0; j < imgWidth; j++)
            {
                var vec = data[j + i * imgWidth];
                var col = new Color32
                {
                    r = vec,
                    g = vec,
                    b = vec,
                    a = 0
                };
                colors[j + i * imgWidth] = col;
            }
        });

        Texture2D tex = new Texture2D(imgWidth, imgHeight, TextureFormat.ARGB32, true, true);
        tex.SetPixels32(colors);
        tex.Apply();
        return tex;
    }
}


/*
# Color of a lake [blue green red]
BGR = np.array([255, 218, 170])
upper = BGR + 10
lower = BGR - 10

image = cv.imread("pond.png")
#cv.imshow('hjelp', image)
#key = cv.waitKey(0)
mask = cv.inRange(image, lower, upper)
#cv.imshow('hjelp', mask)
#key = cv.waitKey(0)

#contours = find_contours(mask)
contours, hierarchy = cv.findContours(mask.copy(), cv.RETR_EXTERNAL, cv.CHAIN_APPROX_SIMPLE)
#print(contours)
copy = list(contours)
copy.sort(key = len, reverse = True)
main_contour = copy[0] #get_main_contour(contours)

cv.drawContours(image, [main_contour], -1, (0, 0, 255), 2)
cv.imshow("contours", image)

key = cv.waitKey(0)

*/