# 🎥 Video Conferencing Setup Guide

This guide will help you set up Azure Communication Services for video calling in your Encadri platform.

## Step 1: Create Azure Communication Services Resource

1. Go to **Azure Portal**: https://portal.azure.com
2. Click **"Create a resource"**
3. Search for **"Communication Services"**
4. Click **Create**
5. Fill in the details:
   - **Subscription**: Your Azure subscription
   - **Resource Group**: `encadri-resources` (or create new)
   - **Resource Name**: `encadri-video-calls`
   - **Data Location**: Choose closest to your users (e.g., `Europe`, `United States`)
6. Click **Review + Create**, then **Create** (takes ~1 minute)

## Step 2: Get Connection String

1. Once created, go to your **Communication Services** resource
2. In the left menu, click **Keys**
3. **Copy the Primary Connection String**
   - It looks like: `endpoint=https://encadri-video-calls.communication.azure.com/;accesskey=YOUR_KEY_HERE`

## Step 3: Add to Railway (Backend)

1. Go to **Railway dashboard**: https://railway.app
2. Select your **Encadri Backend** service
3. Go to **Variables** tab
4. Click **+ New Variable**
5. Add:
   - **Name**: `AZURE_COMMUNICATION_CONNECTION_STRING`
   - **Value**: Paste the connection string you copied
6. Click **Add** and **Deploy**

## Step 4: Install Frontend Packages

Run these commands in the `Encadri-Frontend` folder:

```bash
cd Encadri-Frontend
npm install @azure/communication-calling@1.24.1
npm install @azure/communication-common@2.3.1
```

## Step 5: Test the Video Call Feature

1. **Deploy**: Push code to trigger Vercel deployment
2. **Login** to your app
3. Go to **Meetings** page
4. Click **"🎥 Join Video Call"** on any meeting
5. You should see:
   - Camera preview in waiting room
   - "Join Meeting" button
   - Video controls after joining

## Features Implemented ✅

### Video Call UI
- ✅ **Waiting room** with camera/mic preview
- ✅ **Video grid** for multiple participants
- ✅ **Camera toggle** (on/off)
- ✅ **Microphone toggle** (mute/unmute)
- ✅ **Screen sharing** button
- ✅ **End call** button
- ✅ **Participant count** display
- ✅ **Beautiful dark theme** UI

### Backend API
- ✅ **Token generation** endpoint (`/api/videocall/token`)
- ✅ **Health check** endpoint
- ✅ **Azure Communication Services** integration

## How It Works

```
User clicks "Join Video Call"
    ↓
Frontend requests token from backend
    ↓
Backend uses Azure Communication Services to create user & generate token
    ↓
Frontend receives token
    ↓
User joins video call using token
    ↓
Azure handles video/audio streaming
```

## Testing Checklist

- [ ] Backend deployed on Railway with connection string
- [ ] Frontend packages installed (`npm install`)
- [ ] Can click "Join Video Call" button
- [ ] Waiting room shows camera preview
- [ ] Can join meeting
- [ ] Can toggle camera on/off
- [ ] Can toggle microphone on/off
- [ ] Can end call

## Troubleshooting

### Backend Error: "Connection string not found"
- Make sure you set `AZURE_COMMUNICATION_CONNECTION_STRING` in Railway
- Restart the Railway deployment

### Frontend Error: "Cannot find module @azure/communication-calling"
- Run `npm install @azure/communication-calling@1.24.1`
- Restart `npm start`

### Video doesn't show
- Check browser permissions (camera/microphone)
- Make sure you're using HTTPS (required for camera access)
- Try a different browser (Chrome works best)

## Cost Estimate

Azure Communication Services pricing for video calls:

- **Group video calls**: $0.004 per minute per participant
- **Example**: 10 meetings/day, 30 min each, 2 people = ~$12/month
- **Free tier**: First 10,000 minutes free!

## Next Steps (Optional Enhancements)

1. **Recording**: Record meetings to Azure Blob Storage
2. **Transcription**: Add real-time captions using Azure Speech Services
3. **Screen sharing**: Implement screen share functionality
4. **Chat**: Add in-meeting text chat
5. **Waiting room**: Add supervisor approval before joining
6. **Meeting links**: Share meeting URLs for external participants

## Support

If you encounter issues:
1. Check Railway logs for backend errors
2. Check browser console for frontend errors
3. Verify Azure Communication Services is active in Azure Portal
4. Ensure connection string is correctly set

---

**Congratulations! 🎉**
You now have video conferencing built directly into your platform!
