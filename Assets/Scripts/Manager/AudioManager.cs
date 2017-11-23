using System;
using UnityEngine;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using Game.Const;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Manager {

    public class AudioManager : Singleton<AudioManager> {
        private static readonly object m_Lock;
        private const int SOUNDS_PLAYER_NUM = 5; // 音效播放器数量

        private bool playMusic = true;
        private bool playSound = true;
        private string musicName = "";
        private Dictionary<String, AudioClip> clips = new Dictionary<String, AudioClip>();
        private Dictionary<String, float> playTimes = new Dictionary<String, float>(); // 播放时间列表

        private AudioSource musicPlayer; // 背景音乐
        private AudioPlayer[] soundsPlayers; // 音效
        private int curSoundsPlayerIndex; // 当前音效播放器索引
        private int curId = 0; // 当前音效ID

        public AudioManager() {
        }

        public void Init() { 
			Camera camera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            if (camera != null) {
                musicPlayer = camera.gameObject.AddComponent<AudioSource>();
                musicPlayer.loop = true;
                soundsPlayers = new AudioPlayer[SOUNDS_PLAYER_NUM];
                for (int i = 0; i < SOUNDS_PLAYER_NUM; ++i) {
                    soundsPlayers[i] = new AudioPlayer(); 
                    soundsPlayers[i].source = camera.gameObject.AddComponent<AudioSource>();
                }
            }
        }

        public void PlayMusic(string name) {
            musicName = name;
            if (!playMusic) return;

            if (musicPlayer.clip != null 
                && musicPlayer.clip.name.Equals(musicName)) {
                if (!musicPlayer.isPlaying)
                    musicPlayer.Play();
                return;
            }

            if (clips.ContainsKey(name)) {
                StartPlayMusic(clips[name]);
            } else {
                LoadMusic(name);
            }
        }

        private void LoadMusic(string name) {
            AudioClip clip = Resources.Load("Audio/" + name) as AudioClip;
            if (!clips.ContainsKey(name)) {
                clips.Add(name, clip);
            }
            StartPlayMusic(clip);
        }

        private void StartPlayMusic(AudioClip clip) {
            if (clip != null) {
                if (clip != musicPlayer.clip) 
                    musicPlayer.clip = clip;
                
                if (playMusic) {
                    musicPlayer.Play();
                } else {
                    musicPlayer.Stop();
                }
            }
        }

        public void StopMusic() {
            musicPlayer.Stop();
        }

        public void StopSound(int id) {
            for (int i = 0; i < soundsPlayers.Length; ++i) { 
                AudioPlayer player = soundsPlayers[i];
                if (player.id == id) {
                    player.source.Stop();
                    return;
                }
            }
        }

        public int PlaySound(string name, int priority = 0) {
            name = name.Trim();
            if (name.Equals("") || !playSound) return - 1;
            
            if (playTimes.ContainsKey(name)) {
                if (Time.time - playTimes[name] <= 0.1f) {
                    return - 1;
                }
            }
            playTimes[name] = Time.time;

            if (clips.ContainsKey(name)) {
                StartPlaySound(clips[name], priority);
            } else {
                LoadSound(name, priority);
            }
            return curId;
        }

        private void LoadSound(string name, int priority) {
            AudioClip clip = Resources.Load("Audio/" + name) as AudioClip;
            if (!clips.ContainsKey(name)) {
                clips.Add(name, clip);
            }
            StartPlaySound(clip, priority);
        }

        private void StartPlaySound(AudioClip clip, int priority) {
            if (clip == null) return;

            bool isPlay = false;
            int curIndex = curSoundsPlayerIndex;
            for (int i = 0; i < SOUNDS_PLAYER_NUM; ++i) {
                ++curIndex;
                if (curIndex >= SOUNDS_PLAYER_NUM) {
                    curIndex = curIndex - SOUNDS_PLAYER_NUM;
                }
                if ((soundsPlayers[curIndex].source.clip == null || !soundsPlayers[curIndex].source.isPlaying)
                    || priority >= soundsPlayers[curIndex].priority) {
                    curSoundsPlayerIndex = curIndex;
                    isPlay = true;
                    break;
                }
            }
            if (!isPlay) return;
            
            ++curId;

            soundsPlayers[curSoundsPlayerIndex].source.clip = clip;
            soundsPlayers[curSoundsPlayerIndex].priority = priority;
            soundsPlayers[curSoundsPlayerIndex].id = curId;
            if (playSound) {
                soundsPlayers[curSoundsPlayerIndex].source.Play();
            } else {
                soundsPlayers[curSoundsPlayerIndex].source.Stop();
            }
        }
    }

    public class AudioPlayer {
        
        public AudioSource source;
        public int priority;
        public int id;

        public AudioPlayer() {
        }
    }
}