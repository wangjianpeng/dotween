// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/28 11:20
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php
namespace DG.Tweening
{
    /// <summary>
    /// Spiral tween mode
    /// </summary>
    public enum SpiralMode
    {
        /// <summary>The spiral motion will expand outwards for the whole the tween</summary>
        Expand,
        /// <summary>The spiral motion will expand outwards for half the tween and then will spiral back to the starting position</summary>
        ExpandThenContract
    }
}