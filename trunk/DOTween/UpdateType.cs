// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 17:46
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening
{
    /// <summary>
    /// Update type
    /// </summary>
    public enum UpdateType
    {
        /// <summary>Updates every frame using Unity's timeScale as a base</summary>
        Default,
        /// <summary>Updates every frame but ignores Unity's timeScale</summary>
        Independent
    }
}