using System;
using System.Collections.Generic;
using System.Linq;

namespace FileComparer
{
    public class FileInfoWrapperComparer
    {
        public static IEqualityComparer<FileInfoWrapper> LengthComparer { get; } = new FileInfoWrapperComparerByLength();

        public static IEqualityComparer<FileInfoWrapper> HashComparer { get; } = new FileInfoWrapperComparerByHash();

        private abstract class FileInfoWrapperComparerBase : IEqualityComparer<FileInfoWrapper>
        {
            public bool Equals(FileInfoWrapper x, FileInfoWrapper y)
            {
                if (ReferenceEquals(x, null))
                {
                    return ReferenceEquals(y, null);
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return this.EqualsImplSafe(x, y);
            }

            public int GetHashCode(FileInfoWrapper obj)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException();
                }

                return this.GetHashCodeImplSafe(obj);
            }

            protected abstract int GetHashCodeImplSafe(FileInfoWrapper obj);

            protected abstract bool EqualsImplSafe(FileInfoWrapper x, FileInfoWrapper y);
        }

        private class FileInfoWrapperComparerByLength : FileInfoWrapperComparerBase
        {
            protected override int GetHashCodeImplSafe(FileInfoWrapper obj) => obj.FileInfo.Length.GetHashCode();

            protected override bool EqualsImplSafe(FileInfoWrapper x, FileInfoWrapper y) => x.FileInfo.Length.Equals(y.FileInfo.Length);
        }

        private class FileInfoWrapperComparerByHash : FileInfoWrapperComparerBase
        {
            protected override int GetHashCodeImplSafe(FileInfoWrapper obj) => obj.FileInfo.Length.GetHashCode();

            protected override bool EqualsImplSafe(FileInfoWrapper x, FileInfoWrapper y) => x.FileHash.SequenceEqual(y.FileHash);
        }
    }
}
