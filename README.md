# JobTrack — Full-Stack App

Angular 21 + ASP.NET Core 10 + PostgreSQL

---

## Run Locally

### Prerequisites
- .NET 10 SDK
- Node.js 20+
- Docker Desktop

### Backend

```bash
cd engprac-fullstack-be
docker compose up -d        # start PostgreSQL
dotnet run                  # http://localhost:5118
```

Swagger: http://localhost:5118/swagger

### Frontend

```bash
cd engprac-fullstack-fe
npm install
npm start                   # http://localhost:4200
```

Demo accounts:
- Admin: `alice@example.com` / `password123`
- User: `bob@example.com` / `password123`

---

## Deploy to Production

### ลำดับ deploy

```
Step 1 → Neon   (Database)
Step 2 → Render (Backend)
Step 3 → แก้ environment.prod.ts
Step 4 → Vercel (Frontend)
Step 5 → อัปเดต CORS บน Render
```

---

### Step 1 — Neon (Database)

1. สมัคร https://neon.tech → **Create Project**
2. Region: `ap-southeast-1` (Singapore)
3. Copy **Connection String**:
   ```
   postgresql://user:pass@ep-xxxx.ap-southeast-1.aws.neon.tech/neondb?sslmode=require
   ```

---

### Step 2 — Render (Backend)

1. สมัคร https://render.com → **New → Web Service**
2. Connect GitHub repo → Root Directory: `engprac-fullstack-be`
3. Runtime: **Docker** | Region: **Singapore**
4. เพิ่ม Environment Variables:

| Key | Value |
|-----|-------|
| `ConnectionStrings__DefaultConnection` | `<Neon connection string>` |
| `JwtSettings__Secret` | `<random string 32+ ตัว>` |
| `AllowedOrigins` | `https://your-app.vercel.app` *(ใส่ทีหลังได้)* |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

5. Deploy → Copy URL เช่น `https://jobtrack-api.onrender.com`

> Render free tier จะ sleep หลังไม่มี traffic 15 นาที — request แรกจะช้า ~30 วิ

---

### Step 3 — แก้ Frontend ก่อน deploy

แก้ไฟล์ `engprac-fullstack-fe/src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://jobtrack-api.onrender.com/api'  // ← URL จาก Step 2
};
```

---

### Step 4 — Vercel (Frontend)

1. สมัคร https://vercel.com → **New Project**
2. Connect GitHub repo → Root Directory: `engprac-fullstack-fe`
3. Settings:
   - **Framework Preset**: Angular
   - **Build Command**: `npm run build`
   - **Output Directory**: `dist/frontend/browser`
4. Deploy → Copy URL เช่น `https://jobtrack.vercel.app`

---

### Step 5 — อัปเดต CORS บน Render

Render Dashboard → Environment → แก้:

```
AllowedOrigins = https://jobtrack.vercel.app
```

Save → Render redeploy อัตโนมัติ ✅

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular 21, PrimeNG 21, Signals |
| Backend | ASP.NET Core 10, EF Core, JWT |
| Database | PostgreSQL (Docker local / Neon prod) |
| Auth | JWT Bearer + BCrypt |
