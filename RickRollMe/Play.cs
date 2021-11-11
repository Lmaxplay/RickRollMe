using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using RickRollMe;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Util;
using MediaToolkit.Model;
using NAudio;
using NAudio.Wave;
using System.Threading;

namespace RickRollMe
{
    class Play
    {
        public static void PlayRickRoll()
        {
            string source = Path.GetTempPath();
            YouTube youtube = YouTube.Default;
            YouTubeVideo vid = youtube.GetVideo("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
            File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

            var inputFile = new MediaFile { Filename = source + vid.FullName };
            var outputFile = new MediaFile { Filename = $"{source + vid.FullName}.mp3" };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }
            using (var mf = new MediaFoundationReader($"{source + vid.FullName}.mp3"))
            using (var wo = new WaveOutEvent())
            {
                wo.Init(mf);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
                File.Delete(source + vid.FullName);
            }
        }
    }
}
