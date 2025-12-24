# Quick Azure Deployment - 5 Minutes

## Prerequisites

- Azure account with free credits
- Azure CLI installed: `az --version`

---

## Deploy Backend (2 minutes)

### 1. Login

```bash
az login
```

### 2. Create Resources

```bash
# Create resource group
az group create --name encadri-rg --location eastus

# Create app service plan (F1 Free)
az appservice plan create \
  --name encadri-plan \
  --resource-group encadri-rg \
  --location eastus \
  --sku F1 \
  --is-linux

# Create web app
az webapp create \
  --name encadri-api \
  --resource-group encadri-rg \
  --plan encadri-plan \
  --runtime "DOTNETCORE:8.0"
```

### 3. Configure

```bash
# Set environment
az webapp config appsettings set \
  --name encadri-api \
  --resource-group encadri-rg \
  --settings ASPNETCORE_ENVIRONMENT=Production

# Enable CORS
az webapp cors add \
  --name encadri-api \
  --resource-group encadri-rg \
  --allowed-origins "*"
```

### 4. Deploy Code

```bash
cd Encadri-Backend/Encadri-Backend
dotnet publish -c Release -o ./publish
cd publish
zip -r deploy.zip .

az webapp deployment source config-zip \
  --name encadri-api \
  --resource-group encadri-rg \
  --src deploy.zip
```

**Backend URL**: https://encadri-api.azurewebsites.net

---

## Deploy Frontend (3 minutes)

### Method: Azure Portal (Easiest)

1. Go to https://portal.azure.com
2. Click **Create a resource**
3. Search: **Static Web App**
4. Click Create

**Configuration:**
```
Basics:
  Resource Group: encadri-rg
  Name: encadri-frontend
  Plan type: Free
  Region: East US 2

Deployment:
  Source: GitHub
  GitHub account: [Authorize]
  Organization: bahadhay
  Repository: encadri-V1
  Branch: main

Build:
  Build Presets: Angular
  App location: /Encadri-Frontend
  Api location: [leave empty]
  Output location: dist/encadri-frontend/browser
```

5. Click **Review + Create**
6. Click **Create**

Azure creates GitHub Action automatically.

**Wait 3-5 minutes** for first build.

**Frontend URL**: https://encadri-frontend-xxxxx.azurestaticapps.net

---

## Configure Database

### Option 1: Supabase (Free)

1. Go to https://supabase.com
2. Create project
3. Copy PostgreSQL connection string
4. Add to Azure:

```bash
az webapp config appsettings set \
  --name encadri-api \
  --resource-group encadri-rg \
  --settings DATABASE_URL="postgresql://user:pass@host:5432/dbname"
```

### Option 2: Railway (Free)

1. Go to https://railway.app
2. New Project > PostgreSQL
3. Copy connection string
4. Add to Azure (same command as above)

---

## Verify

### Test Backend

```bash
curl https://encadri-api.azurewebsites.net/swagger
```

### Test Frontend

Open in browser:
```
https://encadri-frontend-xxxxx.azurestaticapps.net
```

---

## Update Backend

```bash
cd Encadri-Backend/Encadri-Backend
dotnet publish -c Release -o ./publish
cd publish
zip -r deploy.zip .

az webapp deployment source config-zip \
  --name encadri-api \
  --resource-group encadri-rg \
  --src deploy.zip
```

## Update Frontend

```bash
git push origin main
```

GitHub Action redeploys automatically.

---

## Delete Everything

```bash
az group delete --name encadri-rg --yes
```

---

## Troubleshooting

**Backend not responding:**
```bash
az webapp log tail --name encadri-api --resource-group encadri-rg
```

**Frontend build failed:**
- Check GitHub Actions tab
- Review build logs

**CORS error:**
- Add specific origin instead of `*`
- Enable credentials

**Database connection:**
- Verify connection string
- Check firewall rules
