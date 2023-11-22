using System;

namespace ChooChoo
{
  public readonly struct TrainDistributableGood : IComparable<TrainDistributableGood>
  {
    private readonly GoodsStationGoodDistributionSetting _goodDistributionSetting;

    public int Stock { get; }

    public int Capacity { get; }

    public TrainDistributableGood(
      int stock,
      int capacity,
      GoodsStationGoodDistributionSetting goodDistributionSetting)
    {
      Stock = stock;
      Capacity = capacity;
      _goodDistributionSetting = goodDistributionSetting;
    }

    public float FillRate
    {
      get
      {
        if (Capacity != 0)
          return Stock / (float) Capacity;
        return Stock != 0 ? 1f : 0.0f;
      }
    }

    public bool CanExport => Stock > 0;

    public float MaxExportAmount => Stock;

    public int FreeCapacity => Capacity - Stock;

    public string GoodId => _goodDistributionSetting.GoodId;

    public void UpdateLastImportTimestamp(float timestamp) => _goodDistributionSetting.LastImportTimestamp = timestamp;

    public int CompareTo(TrainDistributableGood other)
    {
      int num = FillRate.CompareTo(other.FillRate);
      return num != 0 ? num : _goodDistributionSetting.LastImportTimestamp.CompareTo(other._goodDistributionSetting.LastImportTimestamp);
    }

    private float ExportRate => FillRate - _goodDistributionSetting.MaxCapacity;
  }
}
