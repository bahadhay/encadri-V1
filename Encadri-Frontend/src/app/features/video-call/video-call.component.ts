import { Component, OnInit, OnDestroy, inject, signal, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoCallService } from '../../core/services/video-call.service';
import { UiButtonComponent } from '../../shared/components/ui-button/ui-button.component';
import { UiCardComponent } from '../../shared/components/ui-card/ui-card.component';

// These will be imported after npm install
declare var AzureCommunication: any;
declare var Calling: any;

@Component({
  selector: 'app-video-call',
  standalone: true,
  imports: [CommonModule, UiButtonComponent, UiCardComponent],
  templateUrl: './video-call.component.html',
  styleUrls: ['./video-call.component.css']
})
export class VideoCallComponent implements OnInit, OnDestroy {
  private videoCallService = inject(VideoCallService);

  @Input() meetingId?: string;
  @Input() meetingTitle?: string;

  // State
  isCallActive = signal(false);
  isConnecting = signal(false);
  error = signal<string>('');
  participantCount = signal(0);
  isMuted = signal(false);
  isVideoOn = signal(true);

  // Azure Communication Services objects
  private callAgent: any;
  private deviceManager: any;
  private call: any;
  private localVideoStream: any;

  async ngOnInit() {
    // Note: ACS SDK needs to be loaded via script tag in index.html
    // We'll add instructions for this
  }

  async ngOnDestroy() {
    await this.endCall();
  }

  /**
   * Start a video call
   */
  async startCall() {
    this.isConnecting.set(true);
    this.error.set('');

    try {
      // Step 1: Get call token from backend
      const tokenResponse = await this.videoCallService.getCallToken().toPromise();
      if (!tokenResponse) {
        throw new Error('Failed to get call token');
      }

      // Step 2: Initialize Azure Communication Calling
      // Note: This requires ACS SDK to be loaded
      // For now, we'll show a placeholder message
      console.log('Call token received:', tokenResponse.token);

      this.isCallActive.set(true);
      this.isConnecting.set(false);

      // TODO: Implement actual ACS calling once SDK is installed
      // This is a placeholder implementation

    } catch (err: any) {
      this.error.set(err.message || 'Failed to start call');
      this.isConnecting.set(false);
    }
  }

  /**
   * End the current call
   */
  async endCall() {
    if (this.call) {
      await this.call.hangUp();
      this.call = null;
    }
    this.isCallActive.set(false);
  }

  /**
   * Toggle microphone mute
   */
  async toggleMute() {
    if (this.call) {
      if (this.isMuted()) {
        await this.call.unmute();
      } else {
        await this.call.mute();
      }
      this.isMuted.set(!this.isMuted());
    }
  }

  /**
   * Toggle video on/off
   */
  async toggleVideo() {
    if (this.call && this.localVideoStream) {
      if (this.isVideoOn()) {
        await this.call.stopVideo(this.localVideoStream);
      } else {
        await this.call.startVideo(this.localVideoStream);
      }
      this.isVideoOn.set(!this.isVideoOn());
    }
  }

  /**
   * Share screen
   */
  async shareScreen() {
    // Implementation for screen sharing
    console.log('Screen sharing requested');
  }
}
