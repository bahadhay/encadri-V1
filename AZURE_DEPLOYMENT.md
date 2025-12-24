# Azure Deployment Guide - Free Tier

## Architecture

**Frontend**: Azure Static Web Apps (Free)
**Backend**: Azure App Service F1 (Free)
**Database**: External PostgreSQL (Supabase/Railway Free) or Azure Free Database

**Total Cost**: $0/month within free tier limits

---

## Prerequisites

- Azure account (Student/Free subscription)
- Azure CLI installed
- GitHub account
- Local project working

---

## Method 1: Automated Deployment (Recommended)

### Step 1: Make Script Executable

```bash
chmod +x deploy-azure.sh
```

### Step 2: Run Deployment Script

```bash
./deploy-azure.sh
```

### Step 3: Configure Database

**Option A: Use External PostgreSQL (Recommended for Free Tier)**

Use Supabase or Railway free PostgreSQL:

```bash
az webapp config appsettings set \
  --name encadri-api \
  --resource-group encadri-rg \
  --settings DATABASE_URL="your-postgres-connection-string"
```

**Option B: Use Azure Database for PostgreSQL**

Note: This costs money. Only if you have credits.

```bash
az postgres flexible-server create \
  --name encadri-db \
  --resource-group encadri-rg \
  --location eastus \
  --admin-user encadriadmin \
  --admin-password YourSecurePassword123! \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32 \
  --version 14
```

### Step 4: Setup Static Web App (Frontend)

1. Go to Azure Portal: https://portal.azure.com
2. Create Resource > Static Web App
3. Configuration:
   - **Resource Group**: encadri-rg
   - **Name**: encadri-frontend
   - **Plan**: Free
   - **Region**: East US 2
   - **Source**: GitHub
   - **Repository**: bahadhay/encadri-V1
   - **Branch**: main
   - **Build Presets**: Angular
   - **App location**: /Encadri-Frontend
   - **Output location**: dist/encadri-frontend/browser

4. Click Review + Create

Azure will automatically:
- Add GitHub Action workflow
- Deploy on every push to main
- Provide custom domain

---

## Method 2: Manual Azure Portal Deployment

### Step 1: Create Resource Group

1. Azure Portal > Resource Groups > Create
2. Name: `encadri-rg`
3. Region: East US
4. Create

### Step 2: Create App Service Plan

1. Create Resource > App Service Plan
2. Name: `encadri-plan`
3. Operating System: Linux
4. Region: East US
5. Pricing Tier: F1 (Free)
6. Create

### Step 3: Create Web App (Backend)

1. Create Resource > Web App
2. Configuration:
   - **Name**: encadri-api (must be globally unique)
   - **Publish**: Code
   - **Runtime**: .NET 8 (LTS)
   - **Operating System**: Linux
   - **Region**: East US
   - **App Service Plan**: encadri-plan (F1)
3. Create

### Step 4: Configure Backend

1. Go to Web App > Configuration
2. Add Application Settings:
   ```
   ASPNETCORE_ENVIRONMENT = Production
   DATABASE_URL = your-database-connection-string
   ```
3. Save

4. Go to CORS
   - Add: `*` (for development)
   - Enable Access-Control-Allow-Credentials

### Step 5: Deploy Backend Code

**Option A: VS Code Extension**

1. Install Azure App Service extension
2. Right-click `Encadri-Backend/Encadri-Backend`
3. Select "Deploy to Web App"
4. Choose encadri-api

**Option B: Azure CLI**

```bash
cd Encadri-Backend/Encadri-Backend
dotnet publish -c Release -o ./publish
cd publish
zip -r ../deploy.zip .
cd ..

az webapp deployment source config-zip \
  --name encadri-api \
  --resource-group encadri-rg \
  --src deploy.zip
```

### Step 6: Create Static Web App (Frontend)

Follow Step 4 from Method 1

---

## Verify Deployment

### Test Backend

```bash
curl https://encadri-api.azurewebsites.net/api/auth/health
```

Open Swagger:
```
https://encadri-api.azurewebsites.net/swagger
```

### Test Frontend

Wait 2-5 minutes for GitHub Action to complete, then visit:
```
https://<your-static-web-app-name>.azurestaticapps.net
```

---

## Database Setup Options

### Option 1: Supabase (Recommended - Free Forever)

1. Go to https://supabase.com
2. Create new project
3. Copy connection string
4. Add to Azure App Settings as `DATABASE_URL`

### Option 2: Railway (Free Tier)

1. Go to https://railway.app
2. Create PostgreSQL database
3. Copy connection string
4. Add to Azure App Settings

### Option 3: Azure Database for PostgreSQL

**⚠️ Costs money - Only if you have credits**

Cheapest option: Flexible Server Burstable tier (~$12/month)

---

## Free Tier Limits

### Azure Static Web Apps (Free)
- ✅ 100 GB bandwidth/month
- ✅ 0.5 GB storage
- ✅ Custom domains
- ✅ SSL certificates
- ✅ No compute limits

### Azure App Service F1 (Free)
- ⚠️ 60 CPU minutes/day
- ⚠️ 1 GB RAM
- ⚠️ 1 GB storage
- ⚠️ No custom domains
- ⚠️ App sleeps after 20 minutes idle
- ⚠️ Maximum 10 apps

### Recommendations for Free Tier

1. **Use external database** (Supabase/Railway)
2. **Optimize backend** to reduce CPU usage
3. **Accept cold starts** (app wakes from sleep)
4. **Monitor usage** in Azure Portal

---

## Troubleshooting

### Backend Not Starting

Check logs:
```bash
az webapp log tail --name encadri-api --resource-group encadri-rg
```

Common issues:
- Database connection string missing
- .NET runtime mismatch
- Missing environment variables

### Frontend Not Building

1. Check GitHub Actions tab in your repository
2. Review build logs
3. Ensure `staticwebapp.config.json` exists
4. Verify build output path: `dist/encadri-frontend/browser`

### CORS Errors

1. Add frontend URL to backend CORS policy
2. Enable credentials in CORS
3. Check browser console for exact error

### Database Connection Failed

1. Verify connection string format
2. Check firewall rules (Azure Database)
3. Test connection string locally first

### App Sleeping (F1 Tier)

- F1 apps sleep after 20 minutes
- First request takes 10-30 seconds to wake
- Upgrade to B1 ($13/month) to avoid sleep

---

## Update Deployment

### Update Backend

```bash
cd Encadri-Backend/Encadri-Backend
dotnet publish -c Release -o ./publish
cd publish
zip -r ../deploy.zip .
cd ..

az webapp deployment source config-zip \
  --name encadri-api \
  --resource-group encadri-rg \
  --src deploy.zip
```

### Update Frontend

Just push to GitHub:
```bash
git push origin main
```

GitHub Action automatically rebuilds and deploys.

---

## Cost Monitoring

Check current usage:
```bash
az consumption usage list --resource-group encadri-rg
```

Set spending limit in Azure Portal to avoid charges.

---

## Clean Up (Delete Everything)

```bash
az group delete --name encadri-rg --yes --no-wait
```

This deletes all resources and stops any charges.
