using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 轨道计算;

namespace Gmap
{
    public partial class Form1 : Form
    {
        AutoSizeFormClass asc = new AutoSizeFormClass();//分辨率适配

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }

        private void GMapControl1_Load(object sender, EventArgs e)
        {
            //this.gMapControl1.MapProvider = GMapProviders.ArcGIS_ShadedRelief_World_2D_Map; // 设置地图源
            //this.gMapControl1.MapProvider = OpenStreet4UMapProvider.Instance; // 设置地图源
            //this.gMapControl1.MapProvider = GMapProviders.BingHybridMap;//指定地图类型; 

            //this.gMapControl1.Manager.Mode = AccessMode.CacheOnly;//地图加载模式

            //String mapPath = Application.StartupPath + "\\map\\Data.gmdb";
            //GMap.NET.GMaps.Instance.ImportFromGMDB(mapPath);

            //this.gMapControl1.MinZoom = 1;   //最小比例
            //this.gMapControl1.MaxZoom = 23; //最大比例
            //this.gMapControl1.Zoom = 15; //当前比例
            //this.gMapControl1.ShowCenter = false; //不显示中心十字点
            //this.gMapControl1.DragButton = System.Windows.Forms.MouseButtons.Left;//左键拖拽地图
            ////this.gMapControl1.Position = new PointLatLng(23, 113);

            ////GMapProviders.ArcGIS_ShadedRelief_World_2D_Map;
            //GMaps.Instance.Mode = AccessMode.ServerAndCache;
            //使用经纬度设置地图中心


            //Bing混合地图
            var gMap = this.gMapControl1;

            gMap.MapProvider = GMapProviders.BingHybridMap;

            //离线模式
            gMap.Manager.Mode = AccessMode.CacheOnly;
            String mapPath = Application.StartupPath + "\\map\\DataExp.gmdb";

            GMap.NET.GMaps.Instance.ImportFromGMDB(mapPath);

            #region test
            GSize gSize = new GSize(300, 200);
            //gMap.MapProvider.Projection.TileSize
            //gMap.SetSize(gSize);

            #endregion


            //地图中心位置
            //gMap.SetPositionByKeywords("beijing, china"); 
            //gMap.Position = new PointLatLng(39.185422, 112.252810);  //中心点的纬度，经度     

            //不显示中心十字点
            gMap.ShowCenter = false;
            //左键拖拽地图
            gMap.DragButton = MouseButtons.Left;
            gMap.MinZoom = 1;   //最小缩放
            gMap.MaxZoom = 18;  //最大缩放
            gMap.Zoom = 1;      //当前缩放
            gMap.Offset(0, 0);
            gMap.Size = new Size(800, 400);
            //gMap.MapProvider.Projection.TileSize = new GSize(300, 200);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var gMap = this.gMapControl1;
            string line1 = "1 43010U 17072A   19087.83440715 -.00000020  00000-0  11001-4 0  9992";
            string line2 = "2 43010  98.7070  28.4332 0000857 351.5815   8.5341 14.19716662 70822";
            DateTime 开始时间 = Convert.ToDateTime("2019/5/19 17:28:33");
            DateTime 结束时间 = Convert.ToDateTime("2019/5/19 17:42:06");
            int 时区 = 8;
            double x = 116.363;
            double y = 40.059;
            double z = 0.4;


            Satellite satellite = new Satellite("FENGYUN 3D",line1,line2,0);
            ////FENGYUN 3D
            位置计算 位置计算1 = new 位置计算(satellite.名字, satellite.line1, satellite.line2);
            卫星位置[] 卫星位置 = 位置计算1.计算预计轨迹(x, y, z, 开始时间.AddHours(-时区), 结束时间.AddHours(-时区), 1);



            //地图由三层组成。最上层:GMapMarker，中间层:GMapOverlay，最底层:GMapControls　
            GMapOverlay gMapOverlay = new GMapOverlay("markers");

            //PointLatLng point = new PointLatLng(卫星位置)
            List<PointLatLng> points = new List<PointLatLng>();
            foreach (var item in 卫星位置)
            {
                PointLatLng point = new PointLatLng(item.星下点纬度, item.星下点经度);
                points.Add(point);
            }

            MapRoute route = new MapRoute(points, "sas");

            if (route != null)
            {
                gMap.Overlays.Clear();
                //将路转换成线 
                GMapRoute r = new GMapRoute(route.Points, route.Name);
                r.Stroke.Width = 2;                  //路径宽度
                r.Stroke.Color = Color.Red;          //路径颜色
                gMapOverlay.Routes.Add(r);           //向图层中添加道路
                gMap.Overlays.Add(gMapOverlay);      //向控件中添加图层  
                //gMap.Zoom = (int)(gMap.Zoom +1)- 1;
                if(gMap.Zoom != gMap.MaxZoom)
                {
                    gMap.Zoom = gMap.
                        Zoom + 1;
                    gMap.Zoom = gMap.Zoom - 1;
                }
                else
                {
                    gMap.Zoom = gMap.Zoom - 1;
                    gMap.Zoom = gMap.Zoom + 1;
                }
                //gMap.ZoomAndCenterRoute(r);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
    }
}
