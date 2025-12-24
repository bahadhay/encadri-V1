# Deploy to Railway + Vercel (Free & Easy)

## Total Time: 5 minutes
## Cost: $0/month

---

## Step 1: Deploy Backend to Railway (2 minutes)

### 1. Create Railway Account
- Go to https://railway.app
- Sign in with GitHub

### 2. Deploy Backend

1. Click **"New Project"**
2. Select **"Deploy from GitHub repo"**
3. Choose: `bahadhay/encadri-V1`
4. Railway detects .NET automatically

### 3. Add PostgreSQL Database

1. In your project, click **"New"**
2. Select **"Database"** → **"PostgreSQL"**
3. Railway creates database and links it automatically
4. Connection string is auto-injected as `DATABASE_URL`

### 4. Configure Root Directory

1. Click on your backend service
2. Go to **Settings**
3. Set **Root Directory**: `Encadri-Backend/Encadri-Backend`
4. Click **"Deploy"**

### 5. Generate Public URL

1. Click **"Settings"** → **"Networking"**
2. Click **"Generate Domain"**
3. Copy URL (example: `encadri-backend-production.up.railway.app`)

**Backend deployed!** ✅

---

## Step 2: Update Frontend to Use Railway Backend

Update `Encadri-Frontend/src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://YOUR-RAILWAY-URL.up.railway.app/api',
  hubUrl: 'https://YOUR-RAILWAY-URL.up.railway.app/hubs'
};
```

Replace `YOUR-RAILWAY-URL` with your actual Railway domain.

Commit and push:
```bash
git add .
git commit -m "Update production API URL for Railway"
git push origin main
```

---

## Step 3: Deploy Frontend to Vercel (2 minutes)

### 1. Create Vercel Account
- Go to https://vercel.com
- Sign in with GitHub

### 2. Import Project

1. Click **"Add New"** → **"Project"**
2. Import `bahadhay/encadri-V1`
3. Configure:

```
Framework Preset: Angular
Root Directory: Encadri-Frontend
Build Command: npm run build -- --configuration production
Output Directory: dist/encadri-frontend/browser
Install Command: npm install
```

4. Click **"Deploy"**

Wait 2-3 minutes for build.

**Frontend deployed!** ✅

---

## Step 4: Verify Deployment

### Test Backend
```bash
curl https://your-railway-url.up.railway.app/swagger
```

### Test Frontend
Open your Vercel URL in browser:
```
https://your-app-name.vercel.app
```

---

## Free Tier Limits

### Railway
- ✅ 500 hours/month (enough for 24/7)
- ✅ PostgreSQL database included
- ✅ No sleep/cold starts
- ✅ Auto-deploy on git push

### Vercel
- ✅ 100 GB bandwidth/month
- ✅ Unlimited deployments
- ✅ Auto-deploy on git push
- ✅ Custom domains free

---

## Update Deployment

### Backend
Just push to GitHub:
```bash
git push origin main
```
Railway redeploys automatically.

### Frontend
Just push to GitHub:
```bash
git push origin main
```
Vercel redeploys automatically.

---

## Troubleshooting

### Backend not starting on Railway

1. Check logs in Railway dashboard
2. Verify Root Directory is set correctly
3. Ensure PostgreSQL is linked

### Frontend build failed on Vercel

1. Check build logs in Vercel dashboard
2. Verify output directory: `dist/encadri-frontend/browser`
3. Test build locally first:
```bash
cd Encadri-Frontend
npm run build -- --configuration production
```

### CORS errors

Railway backend already has CORS configured. If issues:
1. Check Program.cs has AllowAngularApp policy
2. Verify Vercel URL is allowed

---

## Cost Comparison

| Service | Railway + Vercel | Azure Free |
|---------|-----------------|------------|
| Backend Hosting | Free (500h/month) | Free (60 CPU min/day) |
| Database | Free PostgreSQL | Costs money |
| Frontend | Free (100GB/month) | Free |
| Cold Starts | None | Yes (20 min sleep) |
| Auto Deploy | Yes | No |
| **Total** | **$0** | **$0** (but needs external DB) |

---

## Why Railway + Vercel?

✅ **Easier setup** - No Azure CLI needed
✅ **Better uptime** - No sleep/cold starts
✅ **Free database** - PostgreSQL included
✅ **Auto deploy** - Push to GitHub = instant deploy
✅ **Better logs** - Real-time in dashboard
✅ **Student friendly** - Simple UI, no complex config
