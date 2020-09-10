namespace LeetCode.Csharp.Solutions
{
    using System.Collections.Generic;

    public class BrowserHistory
    {
        private List<string> list = new List<string>();
        private int cur = 0;
        private int llen = 0;

        public BrowserHistory(string homepage)
        {
            this.list.Add(homepage);
            this.cur = 0;
            this.llen = this.cur + 1;
        }

        public void Visit(string url)
        {
            if (this.cur + 1 >= list.Count)
            {
                this.list.Add(url);
                this.cur++;
                this.llen = this.cur + 1;
            }
            else
            {
                this.list[this.cur + 1] = url;
                this.cur++;
                this.llen = this.cur + 1;
            }
        }

        public string Back(int steps)
        {
            for (int i = 0; i < steps && this.cur > 0; i++)
            {
                this.cur--;
            }

            return this.list[this.cur];
        }

        public string Forward(int steps)
        {
            for (int i = 0; i < steps && this.cur < this.llen - 1; i++)
            {
                this.cur++;
            }

            return this.list[this.cur];
        }
    }
}
