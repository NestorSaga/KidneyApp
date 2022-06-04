using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Michsky.UI.ModernUIPack;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private Sprite play, pause, mute, unmute;
    [SerializeField] private ButtonManagerIcon playButton, muteButton;
    private bool muted = false;
    private bool playing = false;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    public void togglePlay()
    {
        if(playing)
        {
            playing = false;
            videoPlayer.Pause();
            playButton.buttonIcon = play;
        } else
        {
            playing = true;
            videoPlayer.Play();
            playButton.buttonIcon = pause;
        }
    }

    public void toggleMute()
    {
            if (muted)
            {
                muted = false;
                muteButton.buttonIcon = mute;
            }
            else
            {
                muted = true;
                muteButton.buttonIcon = unmute;
            }
        videoPlayer.SetDirectAudioMute(0, muted);
    }

    
}
