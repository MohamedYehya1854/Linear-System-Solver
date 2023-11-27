namespace eimination
{
    public partial class Form1 : Form
    {
        static Control FindControl(Control parent, string ctlName)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl.Name.Equals(ctlName))
                {
                    return ctl;
                }

                FindControl(ctl, ctlName);
            }
            return null;
        }

        String[] steps = new String[100];
        List<float[,]> stepsArray = new List<float[,]>();
        int index = 0;
        int PerformOperation(float[,] a, int n)
        {
            int i, j, k = 0, c, flag = 0;

            for (i = 0; i < n; i++)
            {
                if (a[i, i] == 0)
                {
                    c = 1;
                    while ((i + c) < n && a[i + c, i] == 0)
                        c++;
                    if ((i + c) == n)
                    {
                        flag = 1;
                        break;
                    }
                    for (j = i, k = 0; k <= n; k++)
                    {
                        float temp = a[j, k];
                        a[j, k] = a[j + c, k];
                        a[j + c, k] = temp;
                    }
                    int x = i + 1;
                    steps[index] = "Exchange row" + x + " and row" + i;
                    printArrays(false, a, n);
                    index++;

                }

                for (j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        float p = a[j, i] / a[i, i];

                        for (k = 0; k <= n; k++)
                            a[j, k] = a[j, k] - (a[i, k]) * p;

                        int x = j + 1;
                        int y = i + 1;
                        int z = i + 1;
                        /*steps[index] = "row" + z + "-> row" + z + " / (" + a[i, i] + " ) ";
                        printArrays(true);
                        index++;*/
                        steps[index] = "row" + x + "-> row" + x + " - (" + p + " ) row " + y;
                        printArrays(false, a, n);
                        index++;
                    }
                }
            }
            return flag;
        }
        void printArrays(bool empty, float[,] a, int n)
        {
            float[,] step = new float[n, n + 1];
            /* if (empty)
             {*/
            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n + 1; k++)
                {
                    step[i, k] = a[i, k];
                }
            }
            // }
            /*  else
              {
                  step = new float[0,0];
              }*/
            stepsArray.Add(step);
        }

        static float[,] finalArr;
        static float[] PrintResult(float[,] a, int n)
        {
            finalArr = new float[n, n + 1];
            float[] result = new float[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = a[i, n] / a[i, i];
                for (int j = 0; j < n + 1; j++)
                    finalArr[i, j] = a[i, j];
            }

            return result;
        }
        static int CheckConsistency(float[,] a,
                            int n, int flag)
        {
            int i, j;
            float sum;
            flag = 3;
            for (i = 0; i < n; i++)
            {
                sum = 0;
                for (j = 0; j < n; j++)
                    sum = sum + a[i, j];
                if (sum == a[i, j])
                    flag = 2;
            }
            return flag;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox1.Items.Clear();
            stepsArray = new List<float[,]>();
            steps = new string[100];
            index = 0;

            Form form = new Form();
            form.Text = "Enter Matrix Data";
            int size = int.Parse(textBox1.Text);

            float[,] array = new float[size, size + 1];
            float[,] initArr = new float[size, size + 1];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size + 1; y++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + x + "" + y;
                    textBox.Text = 0.ToString();
                    textBox.Size = new System.Drawing.Size(50, 20);
                    textBox.Location = new System.Drawing.Point((y + 1) * textBox.Size.Width, (x + 1) * 21);
                    form.Controls.Add(textBox);
                }
            }

            bool canCalc = false;
            Button button = new Button();
            button.Text = "Solve";
            button.Click += new System.EventHandler(btnButton_Click);
            void btnButton_Click(object sender, EventArgs e)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size + 1; y++)
                    {
                        Control crl = FindControl(form, "textBox" + x + y);
                        if (crl is TextBox)
                            if (((TextBox)crl).Text.Length == 0)
                            {
                                canCalc = false;
                                MessageBox.Show("Enter all data");
                                return;
                            }
                            else
                            {
                                canCalc = true;
                                array[x, y] = float.Parse(((TextBox)crl).Text);
                                initArr[x, y] = float.Parse(((TextBox)crl).Text);

                            }
                    }
                }
                if (canCalc == true)
                {
                    int n = size, flag = 0;

                    flag = PerformOperation(array, n);

                    if (flag == 1)
                        flag = CheckConsistency(array, n, flag);

                    if (flag == 3)
                    {
                        listBox2.Items.Add("No Solution Exists");
                    }
                    else if (flag == 2)
                    {
                        listBox2.Items.Add("Infinite Solutions Exists");

                    }

                    else
                    {
                        listBox1.Items.Add("Init Matrix");
                        for (int i = 0; i < n; i++)
                        {
                            String arrayLine = "";
                            for (int j = 0; j < n + 1; j++)
                            {
                                String spacer = "          ";
                                arrayLine += (initArr[i, j] + spacer);
                            }
                            listBox1.Items.Add(arrayLine);
                        }
                        listBox1.Items.Add("\n");
                        for (int i = 0; i < size; i++)
                        {
                            int x = i + 1;
                            listBox2.Items.Add("x" + x + " =  " + PrintResult(array, n)[i]);
                        }
                        for (int k = 0; k < index; k++)
                        {
                            listBox1.Items.Add(steps[k]);
                            for (int i = 0; i < n; i++)
                            {
                                String arrayLine = "";
                                for (int j = 0; j < n + 1; j++)
                                {
                                    String spacer = "          ";
                                    arrayLine += (stepsArray[k][i, j] + spacer);
                                }
                                listBox1.Items.Add(arrayLine);
                            }
                            listBox1.Items.Add("\n");
                        }
                        /*listBox1.Items.Add("Final Matrix");
                        for (int i = 0; i < n; i++)
                        {
                            String arrayLine = "";
                            for (int j = 0; j < n + 1; j++)
                            {
                                String spacer = j == n - 1 ? "    " : "          ";
                                arrayLine += (array[i, j] + spacer);
                            }
                            listBox1.Items.Add(arrayLine);
                        }*/
                    }

                    form.Close();
                }
            }
            button.Location = new System.Drawing.Point(size * 40, (size * 20) + 40);
            form.Controls.Add(button);
            form.Show();
        }

        /*  private void button2_Click(object sender, EventArgs e)
          {
              String[] arr= textBox2.Text.Split('/');
             // double x = int.Parse(arr[0]) / int.Parse(arr[0]);
              double x = (double)2 /3;
              MessageBox.Show(x.ToString());
          }*/
    }
}