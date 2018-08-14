using System;
using System.IO;
namespace sceneimage
{
    class Program
    {
        ///定义默认1屏宽度
        private static int defaultWidth = 1920;
        ///定义默认1屏高度
        private static int defaultHeight = 1080;
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //运行主逻辑
            mainProcess();   
        }

        static void mainProcess(){
            //屏幕上输出信息
            Console.WriteLine("您需要创建几屏的图片，请输入数字后回车：");
            //等待输入
            string inputValue = Console.ReadLine();
            try{
                //把输入的转换为数字
                double ratio = Convert.ToDouble(inputValue);
                //取开方后作为长和宽的比例 （这样放大比例是一样的）
                ratio = Math.Sqrt(ratio);
                //算出新的宽度和高度，并且取整数
                double width = Math.Ceiling(defaultWidth * ratio);
                double height = Math.Ceiling(defaultHeight * ratio);
                
                //发现一个问题，如果不把宽/高 取为4的倍数，创建的文件会打不开。
                //所以取整为4的倍数
                int clearWidth = (int)(width/4)  * 4;
                int clearHeight = (int)(height/4)  * 4;
                //显示一下宽和高
                Console.WriteLine(string.Format("即将创建图片 宽度{0}，高度{1}", clearWidth, clearHeight));
                createImage(clearWidth, clearHeight);
            }
            catch(Exception ex){
                //屏幕上输出信息
                Console.WriteLine("出现了错误，重新开始。 错误信息："+ex.Message);
                //重新运行
                mainProcess();
            }
        }

        //创建bmp文件
        static void createImage(int width, int height){
            int filesize = width * height * 3; // 24位色深，所以 x3
 
            string targetName = "./sceneImage.bmp";
            if(File.Exists(targetName)){
                File.Delete(targetName);
            }
            FileStream fs = File.OpenWrite(targetName);
            //write bmp header
            fs.WriteByte(0x42);
            fs.WriteByte(0x4d);
            fs.Write(intTo4Bytes(54 + filesize), 0, 4);  //filesize
            fs.Write(intTo4Bytes(0), 0, 4);
            fs.Write(intTo4Bytes(54), 0, 4);  //offset

            //write bmp info
            fs.Write(intTo4Bytes(40), 0, 4);
            fs.Write(intTo4Bytes(width), 0, 4);
            fs.Write(intTo4Bytes(height), 0, 4);
            fs.Write(intTo2Bytes(1), 0, 2);
            fs.Write(intTo2Bytes(24), 0, 2);
            fs.Write(intTo4Bytes(0), 0, 4);
            fs.Write(intTo4Bytes(0), 0, 4);
            fs.Write(intTo4Bytes(0), 0, 4);
            fs.Write(intTo4Bytes(0), 0, 4);
            fs.Write(intTo4Bytes(0), 0, 4);
            fs.Write(intTo4Bytes(0), 0, 4);
            for(int i = 0; i < filesize/3; i++){
                int color = 0xFFFFFF; //全部填充白色  可以改为其他颜色填充, 比如0x1FA0D1
                fs.Write(intTo3Bytes(color), 0, 3);   
            }
            fs.Flush();
            fs.Close();
        }
        static byte[] intTo4Bytes( int value ) 
        { 
            byte[] src = new byte[4];
            src[3] =  (byte) ((value>>24) & 0xFF);
            src[2] =  (byte) ((value>>16) & 0xFF);
            src[1] =  (byte) ((value>>8) & 0xFF);  
            src[0] =  (byte) (value & 0xFF);				
            return src; 
        }
        static byte[] intTo2Bytes( int value ) 
        { 
            byte[] src = new byte[2];
            src[1] =  (byte) ((value>>8) & 0xFF);  
            src[0] =  (byte) (value & 0xFF);				
            return src; 
        }
        static byte[] intTo3Bytes( int value ) 
        { 
            byte[] src = new byte[3];
            src[2] =  (byte) ((value>>16) & 0xFF);  
            src[1] =  (byte) ((value>>8) & 0xFF);  
            src[0] =  (byte) (value & 0xFF);				
            return src; 
        }
    }
}
