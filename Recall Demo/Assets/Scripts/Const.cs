using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const
{
    public static KeyCode[] Control = { KeyCode.UpArrow , KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
                                       KeyCode.Space, KeyCode.X, KeyCode.C, KeyCode.Z};
    /* 控制按键
     * 0、上
     * 1、下
     * 2、左
     * 3、右
     * 4、跳跃
     * 5、攻击
     * 6、时间回退
     */

    public static float PlayerMoveSpeed = 10;
    public static float PlayerJumpSpeed = 30;

}
