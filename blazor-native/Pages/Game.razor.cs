using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using SkiaSharp.Views.Blazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace SkiaSharpSample.Pages
{
    public partial class Game : ComponentBase
    {
        #region Base64
        static string fighterbase64 = "iVBORw0KGgoAAAANSUhEUgAAADQAAAAgCAYAAABdP1tmAAAACXBIWXMAAAsRAAALEQF/ZF+RAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAGoSURBVHja7Jg9TsMwFMdfUA5Q9wBFygmiHIEsiAlQTlBuUMTEFWinbqhZGRJELxCFE+QIkWCnLgOsZWGwX4JdY79QhfylDo5rW36/vI88b7fbAaWqqnoSx1EUXVCedwTd6Pz7Ry7PNaGiKFbimDE2Fi6z5pxvxPk4jq9cnu8TGWqqICUqdX2wT/0KPD5E0vjktIA++FBncu5DtzfwAgCTn+bft9LwdXkPxwMhSh/CUQ0gVhHRrreNeq6CwvSX6yZobfrnhDQ+0tDnx9sQ5Q4qD1ETGQhh5Xk+R4+2NkQ459JeeP8kSa67eOVmRAbG+y6sK4UWAoCq55EYahljygOWd3IhcHZZgmZ/qXjlnCvjJibod0zAdX5rEPSyLJurCGgsaOojjTFWEAQm/28Q9A+MiC3BhTYomBJxLd35mKBtYl3v+WW6tw8AQGDTf9BeqK5rlcU2uDrGXZ4wDKX1ZVkqoxSuvjEBHTG/xVIjVeJsmadQanC+HBRMMzG1bL+HelfLDRf6dz0F07zVt57CTBHdurlQs8vjVrbEXBESLTtGc7jp+EyZ174GAKKck+vBFgIBAAAAAElFTkSuQmCC";
        static string enemybase64 = "iVBORw0KGgoAAAANSUhEUgAAAEwAAAAWCAYAAABqgnq6AAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAJ5SURBVHja7Fi/b9NAFH5pzdKFJmZDaqRS9uCuCBGFKRJSWiyWLijAhhjCwgADEkMXvLKAR4YqbTOxRbVoJKb4H2iJlKAOSDFOK8iCmrAc0bsnfJeL3TOK+CRL/u5yv95977uLM+PxGHTivFr9gvmi617T2T4uFiAdrLInrfYzw9A9oLe15aHFeiVS7/v+PuaWZW1gvl8uNwCgxmjD1jz/tBQmQ4U9/xyUFdZsNt9jns1mcyJFUBQKhQKmf+lPNh6my7L51uv1N6Q916ZUKj3UkZLViPKGYj8We0RKSwK1iHJXtaOM7JSknhKG4XccMLLjDYWUAwCAfD7PVXS7XSDjibijGjAyXzcMwwEusG37WRIKqwh2KHeByphVOTSgDknjakSmOLFT8lav0sP80wonohz1LKpImYLignoeVSRVDPW0BwYXc8MGO5VrBY7qisSnkoQLAAOtp2QQBG1S1MLkpn8X0/bQOgHRKclOOStKEXFBFDWQeVCxWLzNFRzy9Uvvrr7AfPjo5PU0CrMi3ucdTwl/KVVY/vBGCwdp1D9PNmfe8vF/tR0otf+4V+R4efNAqX3c9RkaveUP7jBfSwI+AHg6JZjp9/ttcor1RNcC4hnK95gnj6GLA3aZ3NVPJZY9/Mkp0nc/mOui31NPjru+BfiPeDd99r1p8ulkdPqDqz/79pXbYbP1eV1lQLbjEw9xtk2hwoiiqGc5MkVTxF2fkdJG+ej9ioKnafestEx/AtPkPaf2PGjjgNF/Ap1OB1NPVVG6AoZnOQKANfZ+DABnF6y4nmAuSWHm9Um/Vvy6d/8Id3hpd+f6PJm46vqmTcnjOT/8pl7f7wEA/c39mc8uLG8AAAAASUVORK5CYII=";
        static string boombase64 = "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";
        #endregion
        #region vars
        bool left = false;
        bool right = false;
        bool space = false;
        bool up = false;
        bool down = false;
        bool download = false;
        bool pointerinform = false;
        bool newfighter = true;
        bool SomethingBoomed;
        bool completed = false;
        int cats = 0;
        int dead = 0;
        int tickIndex = 0;
        int youwon = 0;
        long tickSum = 0;
        long[] tickList = new long[100];
        long lastTick = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        List<Tuple<int, int>> bulets = new List<Tuple<int, int>>();
        List<Tuple<int, int>> enemiesbulets = new List<Tuple<int, int>>();
        List<Tuple<int, float, bool, DateTime>> enemies = new List<Tuple<int, float, bool, DateTime>>();
        List<Tuple<int, int, int>> explodes = new List<Tuple<int, int, int>>();
        List<Tuple<string, int, int, int, bool>> unlocked = new List<Tuple<string, int, int, int, bool>>();
        List<Tuple<string, bool>> locked = new List<Tuple<string, bool>>();
        SKBitmap fighter = SKBitmap.Decode(Convert.FromBase64String(fighterbase64));
        SKBitmap enemy = SKBitmap.Decode(Convert.FromBase64String(enemybase64));
        SKBitmap boom = SKBitmap.Decode(Convert.FromBase64String(boombase64));
        DateTime fired;
        DateTime LastEnemyCreated = DateTime.Now;
        DateTime laststars = DateTime.Now;
        SKPoint fighterpos;
        SKPoint formcenter = new SKPoint();
        SKColor linkcolor = SKColors.LightYellow;
        SKSize screensie;
        Random rand = new Random();
        [Inject] IJSRuntime JS { get; set; } = null!;
        #endregion
        protected override void OnInitialized()
        {
            locked.Add(Tuple.Create("Personal Info:", true));
            locked.Add(Tuple.Create("E-Mail: m.aaref-sh@hotmail.com", false));
            locked.Add(Tuple.Create("Mobile: +963 951 985 386", false));
            locked.Add(Tuple.Create("Nationality: Syrian", false));
            locked.Add(Tuple.Create("Gender: Male", false));
            locked.Add(Tuple.Create("BirthDate: 01/01/1997", false));

            locked.Add(Tuple.Create("Study:", true));
            locked.Add(Tuple.Create("At: Syrian Virtual University", false));
            locked.Add(Tuple.Create("Program: ISE *Bachelor*", false));
            locked.Add(Tuple.Create("Information System Engineering", false));
            locked.Add(Tuple.Create("Attended: 2016", false));
            locked.Add(Tuple.Create("Graduated: 2022", false));

            locked.Add(Tuple.Create("Skills:", true));
            locked.Add(Tuple.Create("Experienced in:", false));
            locked.Add(Tuple.Create("C# - C++ - ASP WebForm/MVC", false));
            locked.Add(Tuple.Create("Intermediate experience in:", false));
            locked.Add(Tuple.Create("SQL - API - Xamatin Forms", false));
            locked.Add(Tuple.Create("Flutter - .Net core", false));
            locked.Add(Tuple.Create("Basics of:", false));
            locked.Add(Tuple.Create("Blazor - PHP - Bootstrap", false));
            locked.Add(Tuple.Create("Java - HTML - CSS - JS", false));

            locked.Add(Tuple.Create("Attended courses:*NO certificates*", true));
            locked.Add(Tuple.Create("CCNA - CEH", false));

            locked.Add(Tuple.Create("Problem solving topics: *practiced on*", true));
            locked.Add(Tuple.Create("Math - Geometry - binary search", false));
            locked.Add(Tuple.Create("Number theory - Two pointers - greedy", false));
            locked.Add(Tuple.Create("Implementation - string - Graph", false));

            locked.Add(Tuple.Create("Languages:", true));
            locked.Add(Tuple.Create("English - fluent", false));
            locked.Add(Tuple.Create("Arabic - native language", false));
            locked.Add(Tuple.Create("Duetch - basics only", false));

            locked.Add(Tuple.Create("Other skills:", true));
            locked.Add(Tuple.Create("strong knowlagde in electronics", false));
            locked.Add(Tuple.Create("ability to work for long hours", false));



            base.OnInitialized();
        }
        void CheckKeys()
        {
            if (up)
            {
                fighterpos.Y -= 5;
                fighterpos.Y = Math.Max(20, fighterpos.Y);
            }
            if (down)
            {
                fighterpos.Y += 5;
                fighterpos.Y = Math.Min(screensie.Height - fighter.Height, fighterpos.Y);
            }
            if (right)
            {
                fighterpos.X += 8;
                fighterpos.X = Math.Min(screensie.Width - fighter.Width / 2, fighterpos.X);
            }
            if (left)
            {
                fighterpos.X -= 8;
                fighterpos.X = Math.Max(-fighter.Width / 2, fighterpos.X);
            }
            if (space)
            {
                if (DateTime.Now - fired > TimeSpan.FromMilliseconds(330))
                {
                    bulets.Add(Tuple.Create((int)fighterpos.X + fighter.Width / 2, (int)fighterpos.Y));
                    _ = PlayShot();
                    fired = DateTime.Now;
                }
            }
            if (newfighter)
            {
                newfighter = false;
                fighterpos = new SKPoint(screensie.Width / 2, screensie.Height - fighter.Height - 20);
            }
            SomethingBoomed = false;
        }
        protected void PointerDown(PointerEventArgs e)
        {
            int x = (int)e.OffsetX;
            int y = (int)e.OffsetY;
            download |= (completed && x > screensie.Width + 200 && y > screensie.Height - 25);
            if(pointerinform) NavigationManager.NavigateTo("http://google.com",true);
        }
        protected void PointerMoved(PointerEventArgs e)
        {
            int x = (int)e.OffsetX;
            int y = (int)e.OffsetY;
            linkcolor = (completed && x > screensie.Width + 200 && y > screensie.Height - 25)?SKColors.Pink: SKColors.LightYellow;
            
            pointerinform = (download && x > formcenter.X - 200 && x < formcenter.X + 200 && y > formcenter.Y - 100 && y < formcenter.Y + 100);
        }
        protected void KeyDown(KeyboardEventArgs e)
        {
            if (e.Key is "ArrowRight" or "d") right = true;
            if (e.Key is "ArrowLeft" or "a") left = true;
            if (e.Key is "ArrowUp" or "w") up = true;
            if (e.Key is "ArrowDown" or "s") down = true;
            if (e.Key is " ") space = true;
        }
        protected void KeyUp(KeyboardEventArgs e)
        {
            if (e.Key is "ArrowRight" or "d") right = false;
            if (e.Key is "ArrowLeft" or "a") left = false;
            if (e.Key is "ArrowUp" or "w") up = false;
            if (e.Key is "ArrowDown" or "s") down = false;
            if (e.Key is " ") space = false;
        }
        void DrawDownloadForm(SKCanvas canvas)
        {
            formcenter.X = screensie.Width * 2 / 3;
            formcenter.Y = screensie.Height / 2 - 100;
            SKRect rect = new SKRect(formcenter.X - 200, formcenter.Y - 100, formcenter.X + 200, formcenter.Y + 100);
            SKColor formcolor = SKColors.White;
            if(pointerinform)formcolor = SKColors.LightBlue;
            SKPaint paint = new SKPaint { Color=formcolor,IsStroke=true,StrokeWidth=4 };
            canvas.DrawRect(rect, paint);
            canvas.DrawText("Thanks for reviewing my Bio", formcenter.X - 195, formcenter.Y - 60, new SKPaint{ Color=formcolor,TextSize=24 });
            canvas.DrawLine(formcenter.X - 190, formcenter.Y - 40, formcenter.X + 190, formcenter.Y - 40,new SKPaint { Color=formcolor,StrokeWidth=2 });
            canvas.DrawText("To download CV.pdf click here", formcenter.X - 180, formcenter.Y - 10, new SKPaint{ Color=formcolor,TextSize=20 });
            canvas.DrawText("Please note that if I'd applied to for ", formcenter.X - 180, formcenter.Y+15, new SKPaint{ Color=formcolor,TextSize=14 });
            canvas.DrawText("a role in your company, then this CV will   ", formcenter.X - 180, formcenter.Y+30, new SKPaint{ Color=formcolor,TextSize=14 });
            canvas.DrawText("be such same that the one I submitted.  ", formcenter.X - 180, formcenter.Y+45, new SKPaint{ Color=formcolor,TextSize=14 });
        }
        void DrawEnemies(SKCanvas canvas)
        {

            if (enemies.Count < Math.Min(7, locked.Count) && DateTime.Now - LastEnemyCreated > TimeSpan.FromSeconds(0.5))
            {
                LastEnemyCreated = DateTime.Now;
                enemies.Add(Tuple.Create(rand.Next(0, (int)screensie.Width), -(float)enemy.Height, rand.Next(0, 15) % 2 == 0, DateTime.Now));
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                var enmy = enemies[i];
                bool right = enmy.Item3;
                if (enmy.Item1 + enemy.Width + 10 > screensie.Width && enmy.Item3) right = false;
                if (enmy.Item1 < 10 && !enmy.Item3) right = true;
                canvas.DrawBitmap(enemy, new SKPoint(enmy.Item1, enmy.Item2));
                var lastfire = enmy.Item4;
                if (DateTime.Now - enmy.Item4 > TimeSpan.FromSeconds(rand.Next(2,4)) && enmy.Item2 > 1)
                {
                    enemiesbulets.Add(Tuple.Create(enmy.Item1 + enemy.Width / 2, (int)enmy.Item2 + 4));
                    lastfire = DateTime.Now;
                }

                int mov = 3;
                if (!enmy.Item3) mov = -mov;
                if (screensie.Height - enmy.Item2 < 100) enemies.Remove(enmy);
                else enemies[i] = Tuple.Create(Math.Max(enmy.Item1 + mov, 0), enmy.Item2 + 0.66f, right, lastfire);
            }
        }
        void DrawEnemiesBullet(SKCanvas canvas)
        {
            for (int i = 0; i < enemiesbulets.Count; i++)
            {
                var bulet = enemiesbulets[i];
                bool boomed = false;
                canvas.DrawLine(new SKPoint(bulet.Item1, bulet.Item2), new SKPoint(bulet.Item1, bulet.Item2 + 12), new SKPaint { Color = SKColors.Cyan, StrokeWidth = 4 });
                if (bulet.Item1 > fighterpos.X && bulet.Item1 < fighterpos.X + fighter.Width)
                    if (bulet.Item2 < fighterpos.Y + fighter.Height && bulet.Item2 > fighterpos.Y - 10)
                    {
                        explodes.Add(Tuple.Create((int)fighterpos.X, (int)fighterpos.Y - 10, 25));
                        SomethingBoomed = true;
                        enemiesbulets.Remove(bulet);
                        newfighter = true;
                        boomed = true;
                        dead++;
                        break;
                    }
                if (boomed) continue;
                if (bulet.Item2 > screensie.Height) enemiesbulets.Remove(bulet);
                else enemiesbulets[i] = Tuple.Create(bulet.Item1, bulet.Item2 + 17);
            }
        }
        void DrawExplodes(SKCanvas canvas)
        {
            for (int i = 0; i < explodes.Count; i++)
            {
                var explode = explodes[i];
                canvas.DrawBitmap(boom, new SKPoint(explode.Item1, explode.Item2));
                if (explode.Item3 == 1) explodes.Remove(explode);
                else explodes[i] = Tuple.Create(explode.Item1, explode.Item2, explode.Item3 - 1);
            }
        }
        void DrawBullets(SKCanvas canvas)
        {
            for (int i = 0; i < bulets.Count; i++)
            {
                bool boomed = false;
                var bulet = bulets[i];
                foreach (var enmy in enemies)
                    if (bulet.Item1 < enmy.Item1 + enemy.Width && bulet.Item1 > enmy.Item1 && bulet.Item2 < enmy.Item2 + enemy.Height + 12 && bulet.Item2 > enmy.Item2)
                    {
                        unlocked.Add(Tuple.Create(locked[0].Item1, enmy.Item1, (int)enmy.Item2, 30, locked[0].Item2));
                        locked.RemoveAt(0);
                        explodes.Add(Tuple.Create(enmy.Item1, (int)enmy.Item2 - 10, 25));
                        SomethingBoomed = true;
                        bulets.Remove(bulet);
                        enemies.Remove(enmy);
                        boomed = true;
                        break;
                    }
                if (boomed) continue;
                canvas.DrawLine(new SKPoint(bulet.Item1, bulet.Item2), new SKPoint(bulet.Item1, bulet.Item2 - 12), new SKPaint { Color = SKColors.Yellow, StrokeWidth = 4 });
                if (bulet.Item2 < 20) bulets.Remove(bulet);
                else bulets[i] = Tuple.Create(bulet.Item1, bulet.Item2 - 17);
            }
        }

        void DrawTexts(SKCanvas canvas)
        {
            cats = 0;
            var fps = GetCurrentFPS();
            if (SomethingBoomed) _ = PlayBoom();
            SKPaint paint = new SKPaint { Color = SKColors.White, StrokeWidth = 2 };
            canvas.DrawText($"{fps:0.00}fps", 10, screensie.Height - 10f, paint);
            canvas.DrawText($"You dead {dead} times", 10, screensie.Height - 25f, paint);
            canvas.DrawText("Muhammad Aaref Al-Shami", screensie.Width, 25, new SKPaint { TextSize = 20, Color = SKColors.WhiteSmoke, IsStroke = true });
            for (int i = 0; i < unlocked.Count; i++)
            {
                var info = unlocked[i];
                if (info.Item5) cats++;
                SKPaint infopaint = new SKPaint { TextSize = 13 + (info.Item4 + 1) / 2, Color = SKColors.LightGreen };
                int defx = (int)screensie.Width - info.Item2;
                int x = info.Item2 + defx / info.Item4;
                int defy = 45 + cats * 5 + 15 * i - info.Item3;
                int y = info.Item3 + defy / info.Item4;
                if (info.Item5) infopaint.Color = SKColors.Yellow;

                unlocked[i] = Tuple.Create(info.Item1, info.Item2, info.Item3, Math.Max(info.Item4 - 1, 1), info.Item5);
                canvas.DrawText(info.Item1, x, y, infopaint);
            }
            if (completed)
            {
                canvas.DrawText("You Completed this Game. Greate :)", screensie.Width + 200, screensie.Height - 25, new SKPaint { TextSize = 13, Color = SKColors.LightYellow });
                canvas.DrawText("Click Here to >>Download CV.PDF<<", screensie.Width + 200, screensie.Height - 10, new SKPaint { TextSize = 13, Color = linkcolor });
                if(!download)
                canvas.DrawText("You Won", screensie.Width / 2 - 100, youwon, new SKPaint { StrokeWidth = 5, TextSize = 120, Color = SKColors.LightGray, IsStroke = true });
                else DrawDownloadForm(canvas);
                if (youwon < screensie.Height / 2) youwon+=2;
            }
        }
        void DrawStars(SKCanvas canvas)
        {
            if (DateTime.Now - laststars > TimeSpan.FromSeconds(0.1))
            {
                laststars = DateTime.Now;
                for (int i = 0; i < 20; i++)
                    canvas.DrawPoint(new SKPoint(rand.Next(0, (int)screensie.Width), rand.Next(0, (int)screensie.Height)), new SKPaint { Color = SKColors.White, StrokeWidth = 3 });
            }
        }
        void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var surfaceSize = e.BackendRenderTarget.Size;
            completed = locked.Count == 0;
            screensie = surfaceSize;
            screensie.Width -= 500;

            CheckKeys();
            canvas.Clear(SKColors.Black);
            DrawStars(canvas);
            if (!download)
            {
                DrawExplodes(canvas);
                canvas.DrawBitmap(fighter, fighterpos);
                DrawEnemiesBullet(canvas);
                DrawEnemies(canvas);
                DrawBullets(canvas);
            }
            DrawTexts(canvas);
        }

        async Task PlayBoom() => await JS.InvokeVoidAsync("PlayAudioFile", "/sounds/boom.mp3");
        async Task PlayShot() => await JS.InvokeVoidAsync("PlayAudioFile", "/sounds/shot.mp3");
        double GetCurrentFPS()
        {
            var newTick = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var delta = newTick - lastTick;
            lastTick = newTick;

            tickSum -= tickList[tickIndex];
            tickSum += delta;
            tickList[tickIndex] = delta;

            if (++tickIndex == tickList.Length)
                tickIndex = 0;

            return 1000.0 / ((double)tickSum / tickList.Length);
        }

    }
}
