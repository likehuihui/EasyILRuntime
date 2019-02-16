using System;
using System.Collections.Generic;
using System.Text;


namespace KCBF
{
    /// <summary>
    /// 森林舞会逻辑接口
    /// </summary>
    public partial class KCBF_
    {
        float M_PI = 3.14159265358979323846f;
        float M_PI_2 = 1.57079632679489661923f;
        float M_PI_4 = 0.785398163397448309616f;
        float M_1_PI = 0.318309886183790671538f;
        float M_2_PI = 0.636619772367581343076f;
        public int Factorial(int number)
        {
            int factorial = 1;
            int temp = number;
            for (int i = 0; i < number; ++i)
            {
                factorial *= temp;
                --temp;
            }

            return factorial;
        }

        public int Combination(int count, int r)
        {
            return Factorial(count) / (Factorial(r) * Factorial(count - r));
        }

        public float CalcDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public float CalcAngle(float x1, float y1, float x2, float y2)
        {
            float distance = CalcDistance(x1, y1, x2, y2);
            if (distance == 0.0f) return 0.0f;
            float sin_value = (x1 - x2) / distance;

            if (sin_value < -1)
            {
                sin_value = -1;
            }
            if (sin_value > 1)
            {
                sin_value = 1;
            }

            float angle = (float)Math.Acos(sin_value);
            if (y1 < y2) angle = 2 * M_PI - angle;
            angle += M_PI_2;
            return angle;
        }

        public void BuildLinear(float[] init_x, float[] init_y, int init_count, List<FPoint> trace_vector, float distance)
        {
            trace_vector.Clear();
            for (int i = 0; i < init_count; i++)
            {
                trace_vector.Add(new FPoint() { x = init_x[i], y = init_y[i] });
            }
//             if (init_count < 2) return;
//             if (distance <= 0.0f) return;
// 
//             float distance_total = CalcDistance(init_x[init_count - 1], init_y[init_count - 1], init_x[0], init_y[0]);
//             if (distance_total <= 0.0f) return;
// 
//             float cos_value = Math.Abs(init_y[init_count - 1] - init_y[0]) / distance_total;
// 
//             if (cos_value < -1)
//             {
//                 cos_value = -1;
//             }
//             if (cos_value > 1)
//             {
//                 cos_value = 1;
//             }
// 
//             float angle = (float)Math.Acos(cos_value);
// 
//             FPoint point;
//             point.x = init_x[0];
//             point.y = init_y[0];
//             trace_vector.Add(point);
//             float temp_distance = 0.0f;
// 
//             int size;
//             while (temp_distance < distance_total)
//             {
//                 size = trace_vector.Count;
// 
//                 if (init_x[init_count - 1] < init_x[0])
//                 {
//                     point.x = init_x[0] - (float)Math.Sin(angle) * (distance * size);
//                 }
//                 else
//                 {
//                     point.x = init_x[0] + (float)Math.Sin(angle) * (distance * size);
//                 }
// 
//                 if (init_y[init_count - 1] < init_y[0])
//                 {
//                     point.y = init_y[0] - (float)Math.Cos(angle) * (distance * size);
//                 }
//                 else
//                 {
//                     point.y = init_y[0] + (float)Math.Cos(angle) * (distance * size);
//                 }
// 
//                 trace_vector.Add(point);
//                 temp_distance = CalcDistance(point.x, point.y, init_x[0], init_y[0]);
//             }
// 
//             FPoint temp_point = trace_vector[trace_vector.Count-1];
//             temp_point.x = init_x[init_count - 1];
//             temp_point.y = init_y[init_count - 1];
        }

        public void BuildLinear(float[] init_x, float[] init_y, int init_count, List<FPointAngle> trace_vector, float distance, bool onlyNext, bool clearOutScreen, float screenWidth, float screenHeight)
        {
            trace_vector.Clear();
            for (int i = 0; i < init_count; i++)
            {
                trace_vector.Add(new FPointAngle() { x = init_x[i], y = init_y[i] });
            }

            //if (init_count < 2) return;
            //if (distance <= 0.0f) return;

            //float distance_total = CalcDistance(init_x[init_count - 1], init_y[init_count - 1], init_x[0], init_y[0]);
            //if (distance_total <= 0.0000001f) return;

            //float cos_value = Math.Abs(init_y[init_count - 1] - init_y[0]) / distance_total;
            //if (cos_value < -1)
            //{
            //    cos_value = -1;
            //}
            //if (cos_value > 1)
            //{
            //    cos_value = 1;
            //}
            //float temp_angle = (float)Math.Acos(cos_value);



            //FPointAngle point=new KCBF.KCBF_.FPointAngle ();
            //point.x = init_x[0];
            //point.y = init_y[0];

            //float temp_value = (init_x[1] - init_x[0]) / distance_total;

            //if (temp_value < -1)
            //{
            //    temp_value = -1;
            //}
            //if (temp_value > 1)
            //{
            //    temp_value = 1;
            //}

            //if ((init_y[1] - init_y[0]) >= 0.0f) point.angle = (float)Math.Acos(temp_value);
            //else point.angle = -(float)Math.Acos(temp_value);


            //trace_vector.Add(point);
            //float temp_distance = 0.0f;


            //int size;
            //while (temp_distance < distance_total)
            //{
            //    size = trace_vector.Count;

            //    if (init_x[init_count - 1] < init_x[0])
            //    {
            //        point.x = init_x[0] - (float)Math.Sin(temp_angle) * (distance * size);
            //    }
            //    else
            //    {
            //        point.x = init_x[0] + (float)Math.Sin(temp_angle) * (distance * size);
            //    }

            //    if (init_y[init_count - 1] < init_y[0])
            //    {
            //        point.y = init_y[0] - (float)Math.Cos(temp_angle) * (distance * size);
            //    }
            //    else
            //    {
            //        point.y = init_y[0] + (float)Math.Cos(temp_angle) * (distance * size);
            //    }



            //    trace_vector.Add(point);
            //    if (clearOutScreen)
            //    {
            //        if (point.x < 0 || point.x > screenWidth || point.y < 0 || point.y > screenHeight)
            //        {
            //            break;
            //        }
            //    }
            //    if (onlyNext)
            //        break;

            //    temp_distance = CalcDistance(point.x, point.y, init_x[0], init_y[0]);
            //}
            //if (!onlyNext && !clearOutScreen)
            //{
            //    FPointAngle temp_point = trace_vector[trace_vector.Count-1];
            //    temp_point.x = init_x[init_count - 1];
            //    temp_point.y = init_y[init_count - 1];
            //}
        }

        public void BuildBezier(float[] init_x, float[] init_y, int init_count, List<FPoint> trace_vector, float distance)
        {
            trace_vector.Clear();
            for (int i = 0; i < init_count; i++)
            {
                trace_vector.Add(new FPoint() { x = init_x[i], y = init_y[i] });
            }

            //int index = 0;
            //FPoint temp_pos = new KCBF.KCBF_.FPoint() { x = 0, y = 0 };
            //float t = 0.0f;
            //int count = init_count - 1;
            //float temp_distance = distance;
            //float temp_value = 0.0f;

            //while (t <= 1.0f)
            //{
            //    temp_pos.x = 0.0f;
            //    temp_pos.y = 0.0f;
            //    index = 0;
            //    while (index <= count)
            //    {
            //        temp_value = (float)Math.Pow(t, index) * (float)Math.Pow(1.0f - t, count - index) * Combination(count, index);
            //        temp_pos.x += init_x[index] * temp_value;
            //        temp_pos.y += init_y[index] * temp_value;
            //        ++index;
            //    }

            //    float pos_space = 0.0f;
            //    if (trace_vector.Count > 0)
            //    {
            //        FPoint back_pos = trace_vector[trace_vector.Count-1];
            //        pos_space = CalcDistance(back_pos.x, back_pos.y, temp_pos.x, temp_pos.y);
            //    }

            //    if (pos_space >= temp_distance || trace_vector.Count == 0)
            //    {
            //        trace_vector.Add(temp_pos);
            //    }

            //    t += 0.00001f;
            //}
        }

        public void BuildBezierFast(float[] init_x, float[] init_y, int init_count, List<FPoint> trace_vector, float distance)
        {
            trace_vector.Clear();

            for (int i = 0; i < init_count; i++)
            {
                trace_vector.Add(new FPoint() { x = init_x[i], y = init_y[i] });
            }
            //int index = 0;
            //FPoint temp_pos = new KCBF.KCBF_.FPoint() { x = 0, y = 0 };
            //float t = 0.0f;
            //int count = init_count - 1;
            //float temp_distance = distance;
            //float temp_value = 0.0f;

            //while (t <= 1.0f)
            //{
            //    temp_pos.x = 0.0f;
            //    temp_pos.y = 0.0f;
            //    index = 0;
            //    while (index <= count)
            //    {
            //        temp_value = (float)Math.Pow(t, index) * (float)Math.Pow(1.0f - t, count - index) * Combination(count, index);
            //        temp_pos.x += init_x[index] * temp_value;
            //        temp_pos.y += init_y[index] * temp_value;
            //        ++index;
            //    }

            //    float pos_space = 0.0f;
            //    if (trace_vector.Count > 0)
            //    {
            //        FPoint back_pos = trace_vector[trace_vector.Count-1];
            //        pos_space = CalcDistance(back_pos.x, back_pos.y, temp_pos.x, temp_pos.y);
            //    }

            //    if (pos_space >= temp_distance || trace_vector.Count == 0)
            //    {
            //        trace_vector.Add(temp_pos);
            //    }

            //    t += 0.01f;
            //}
        }

        public float angle_range(float angle)
        {
            while (angle <= -M_PI * 2)
            {
                angle += M_PI * 2;
            }
            if (angle < 0.0f) angle += M_PI * 2;
            while (angle >= M_PI * 2)
            {
                angle -= M_PI * 2;
            }
            return angle;
        }


        public void GetTargetPoint(float screen_width, float screen_height, float src_x_pos, float src_y_pos, float angle, float target_x_pos, float target_y_pos)
        {
            angle = angle_range(angle);

            if (angle > 0.0f && angle < M_PI_2)
            {
                target_x_pos = screen_width + 300;
                target_y_pos = src_y_pos + (screen_width - src_x_pos + 300) *(float)Math.Tan(angle);
            }
            else if (angle >= M_PI_2 && angle < M_PI)
            {
                target_x_pos = -300;
                target_y_pos = src_y_pos - (src_x_pos + 300) * (float)Math.Tan(angle);
            }
            else if (angle >= M_PI && angle < 3 * M_PI / 2.0f)
            {
                target_x_pos = -300;
                target_y_pos = src_y_pos - (src_x_pos + 300) * (float)Math.Tan(angle);
            }
            else
            {
                target_x_pos = screen_width + 300;
                target_y_pos = src_y_pos + (screen_width - src_x_pos + 300) * (float)Math.Tan(angle);
            }
        }
        //创建一条抛物线, a b c 即抛物线的abc参数
        public void BuildParabola(float[] init_x, float[] init_y, int init_count, List<FPointAngle> trace_vector, float distance)
        {
            trace_vector.Clear();
            BuildBezier(init_x, init_y, init_count, trace_vector, distance);
        }

        //转换位置
        public void BuildBezier(float[] init_x, float[] init_y, int init_count, List<FPointAngle> trace_vector, float distance)
        {
            trace_vector.Clear();
            for (int i = 0; i < init_count; i++)
            {
                trace_vector.Add(new FPointAngle() { x = init_x[i], y = init_y[i] });
            }
            //FPointAngle pos1 = new KCBF.KCBF_.FPointAngle() { x = init_x[0], y = init_y[0], angle = 1.0f };
            //trace_vector.Add(pos1);

            //int index = 0;
            //FPointAngle temp_pos0 = new KCBF.KCBF_.FPointAngle() { x = 0, y = 0 };
            //float t = 0.0f;
            //int count = init_count - 1;
            //float temp_distance = distance;
            //FPointAngle temp_pos = new KCBF.KCBF_.FPointAngle() { x = 0, y = 0 };
            //float temp_value = 0.0f;

            //while (t < 1.0f)
            //{
            //    temp_pos.x = 0.0f;
            //    temp_pos.y = 0.0f;
            //    index = 0;
            //    while (index <= count)
            //    {
            //        temp_value = (float)Math.Pow(t, index) * (float)Math.Pow(1.0f - t, count - index) * Combination(count, index);
            //        temp_pos.x += init_x[index] * temp_value;
            //        temp_pos.y += init_y[index] * temp_value;
            //        ++index;
            //    }

            //    float pos_space = 0.0f;
            //    if (trace_vector.Count > 0)
            //    {
            //        FPointAngle back_pos = trace_vector[trace_vector.Count - 1];
            //        pos_space = CalcDistance(back_pos.x, back_pos.y, temp_pos.x, temp_pos.y);
            //    }

            //    if (pos_space >= temp_distance || trace_vector.Count == 0)
            //    {
            //        if (trace_vector.Count > 0)
            //        {
            //            float temp_dis = CalcDistance(temp_pos.x, temp_pos.y, temp_pos0.x, temp_pos0.y);
            //            if (temp_dis != 0.0f)
            //            {
            //                float temp_value1 = (temp_pos.x - temp_pos0.x) / temp_dis;

            //                if (temp_value1 < -1)
            //                {
            //                    temp_value1 = -1;
            //                }
            //                if (temp_value1 > 1)
            //                {
            //                    temp_value1 = 1;
            //                }

            //                if ((temp_pos.y - temp_pos0.y) >= 0.0f) temp_pos.angle = (float)Math.Acos(temp_value1);
            //                else temp_pos.angle = -(float)Math.Acos(temp_value);
            //            }
            //            else
            //            {
            //                temp_pos.angle = 1.0f;
            //            }
            //        }
            //        else
            //        {
            //            temp_pos.angle = 1.0f;
            //        }
            //        trace_vector.Add(temp_pos);
            //        temp_pos0.x = temp_pos.x;
            //        temp_pos0.y = temp_pos.y;
            //    }

            //    t += 0.00001f;
            //}
        }

        public void BuildCircle(float center_x, float center_y, float radius, FPoint[] fish_pos, int fish_count)
        {

            //assert(fish_count > 0);
            if (fish_count <= 0) return;
            float cell_radian = 2 * M_PI / fish_count;

            // x = xo + r * cos(α)
            // y = yo + r * sin(α)
            for (int i = 0; i < fish_count; ++i)
            {
                fish_pos[i].x = center_x + radius * (float)Math.Sin(i * cell_radian);
                fish_pos[i].y = center_y + radius * (float)Math.Cos(i * cell_radian);
            }
        }

        public void BuildCircle(float center_x, float center_y, float radius, FPointAngle[] fish_pos, int fish_count, float rotate, float rotate_speed)
        {
            //assert(fish_count > 0);
            if (fish_count <= 0) return;
            float cell_radian = 2 * M_PI / fish_count;

            // x = xo + r * cos(α)
            // y = yo + r * sin(α)
            FPoint last_pos;
            // 这里计算好像有问题
            for (int i = 0; i < fish_count; ++i)
            {
                last_pos.x = center_x + radius * (float)Math.Cos(i * cell_radian + rotate - rotate_speed);
                last_pos.y = center_y + radius * (float)Math.Sin(i * cell_radian + rotate - rotate_speed);

                fish_pos[i].x = center_x + radius * (float)Math.Cos(i * cell_radian + rotate);
                fish_pos[i].y = center_y + radius * (float)Math.Sin(i * cell_radian + rotate);
                float temp_dis = CalcDistance(fish_pos[i].x, fish_pos[i].y, last_pos.x, last_pos.y);
                if (temp_dis != 0.0f)
                {
                    float temp_value = (fish_pos[i].x - last_pos.x) / temp_dis;
                    if (temp_value < -1)
                    {
                        temp_value = -1;
                    }
                    if (temp_value > 1)
                    {
                        temp_value = 1;
                    }
                    if ((fish_pos[i].y - last_pos.y) >= 0.0f)
                    {
                        fish_pos[i].angle = (float)Math.Acos(temp_value);
                    }
                    else
                    {
                        fish_pos[i].angle = -(float)Math.Acos(temp_value);
                    }
                }
                else
                {
                    fish_pos[i].angle = M_PI_2;
                }
            }

        }


        //获取随机数， 范围在l跟r之间
        public float getRandomFloat(float l, float r)
        {
            Random rand = new Random();
            float random = ((float)rand.NextDouble()) / 0x7fff;

            float dx = (r - l) * random;

            return dx + l;
        }

    }
}
